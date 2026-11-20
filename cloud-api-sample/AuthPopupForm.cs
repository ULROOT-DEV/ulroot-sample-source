using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CloudApiSample.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudApiSample
{
    /// <summary>
    /// 매뉴얼 3.1~3.3장(Application Obtain Users Authorization)에 따라
    /// auth.authorize Request URL을 생성하고, 브라우저 로그인/승인 후 반환되는
    /// userid, usertoken, svrurl, svrrgnid를 수신·파싱하는 팝업 창입니다.
    /// </summary>
    public partial class AuthPopupForm : Form
    {
        private readonly string _serverBaseUrl;
        private readonly string _appKey;
        private readonly string _appToken;

        private HttpListener _callbackListener;
        private CancellationTokenSource _listenerCts;

        /// <summary>인증 완료 후 파싱된 User ID (숫자)</summary>
        public string ResultUserId { get; private set; }

        /// <summary>인증 완료 후 파싱된 User Token</summary>
        public string ResultUserToken { get; private set; }

        /// <summary>인증 완료 후 파싱된 OpenAPI Server URL (svrurl)</summary>
        public string ResultServerUrl { get; private set; }

        /// <summary>인증 완료 후 파싱된 서버 리전 ID (svrrgnid)</summary>
        public string ResultRegionId { get; private set; }

        /// <summary>인증 완료 후 파싱된 토큰 만료 시각(Unix 초)</summary>
        public string ResultExpiredTime { get; private set; }

        public AuthPopupForm(string serverBaseUrl, string appKey, string appToken)
        {
            InitializeComponent();

            _serverBaseUrl = CloudRegionInfo.NormalizeBaseUrl(serverBaseUrl);
            _appKey = (appKey ?? string.Empty).Trim();
            _appToken = (appToken ?? string.Empty).Trim();

            // UserID 입력란은 숫자만 허용합니다.
            txtParsedUserId.KeyPress += TxtParsedUserId_KeyPress;

            cboLang.Items.AddRange(new object[] { "en", "zh", "jp" });
            cboLang.SelectedIndex = 0;

            txtCallbackUrl.Text = "http://localhost:18080/callback/";
            RegenerateAuthorizeUrl();
        }

        private void TxtParsedUserId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void AuthPopupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCallbackListener();
        }

        /// <summary>
        /// 현재 입력된 파라미터(auth_callback, lang, 현재 UTC 시각)를 기준으로 3.1 Request URL과 MD5 서명을 새로 생성합니다.
        /// </summary>
        private void RegenerateAuthorizeUrl()
        {
            string callbackUrl = txtCallbackUrl.Text.Trim();
            if (string.IsNullOrEmpty(callbackUrl))
            {
                txtRequestUrl.Text = string.Empty;
                return;
            }

            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            int messageId = (int)(timestamp % 1000000);
            string lang = cboLang.SelectedItem != null ? cboLang.SelectedItem.ToString() : "en";

            txtRequestUrl.Text = CloudApiSigner.BuildAuthorizeRequestUrl(
                serverBaseUrl: _serverBaseUrl,
                messageId: messageId,
                appKey: _appKey,
                appToken: _appToken,
                authCallback: callbackUrl,
                timestamp: timestamp,
                did: null,
                lang: lang);
        }

        private void btnRegenerateUrl_Click(object sender, EventArgs e)
        {
            RegenerateAuthorizeUrl();
            lblStatus.Text = "상태: 새 타임스탬프와 서명으로 Request URL을 생성했습니다. (유효시간 5분)";
        }

        private void btnCopyUrl_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRequestUrl.Text))
            {
                return;
            }

            Clipboard.SetText(txtRequestUrl.Text);
            lblStatus.Text = "상태: Request URL을 클립보드에 복사했습니다.";
        }

        private void btnOpenBrowser_Click(object sender, EventArgs e)
        {
            // 브라우저를 열기 직전에 최신 타임스탬프로 URL을 재생성합니다 (5분 만료 방지).
            RegenerateAuthorizeUrl();
            string url = txtRequestUrl.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("auth_callback URL을 입력하세요.", "입력 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // auth_callback이 http://localhost:... 형태라면 임시 로컬 HttpListener를 열어 콜백을 자동 수신합니다.
            TryStartLocalCallbackListener(txtCallbackUrl.Text.Trim());

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"기본 브라우저를 열지 못했습니다.\nURL을 복사해 직접 브라우저에 붙여넣으세요.\n\n{ex.Message}", "브라우저 열기 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// auth_callback이 localhost HTTP 주소인 경우, 리다이렉트 결과를 자동 수신하기 위한 임시 로컬 웹서버를 시작합니다.
        /// </summary>
        private void TryStartLocalCallbackListener(string callbackUrl)
        {
            StopCallbackListener();

            if (!Uri.TryCreate(callbackUrl, UriKind.Absolute, out Uri uri))
            {
                return;
            }

            bool isLocal = string.Equals(uri.Host, "localhost", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(uri.Host, "127.0.0.1", StringComparison.OrdinalIgnoreCase);

            if (!isLocal || !string.Equals(uri.Scheme, "http", StringComparison.OrdinalIgnoreCase))
            {
                lblStatus.Text = "상태: 외부 auth_callback 주소입니다. 브라우저 인증 완료 후 리다이렉트된 URL/JSON을 아래에 붙여넣고 [결과 파싱]을 누르세요.";
                return;
            }

            // HttpListener prefix는 반드시 '/'로 끝나야 합니다.
            string prefix = $"{uri.Scheme}://{uri.Host}:{uri.Port}{uri.AbsolutePath}";
            if (!prefix.EndsWith("/", StringComparison.Ordinal))
            {
                prefix += "/";
            }

            try
            {
                var listener = new HttpListener();
                listener.Prefixes.Add(prefix);
                listener.Start();

                _callbackListener = listener;
                _listenerCts = new CancellationTokenSource();
                lblStatus.Text = $"상태: 브라우저 인증 대기 중... ({prefix} 로컬 콜백 수신 대기)";

                Task.Run(() => ListenCallbackOnceAsync(listener, _listenerCts.Token));
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"상태: 로컬 콜백 서버 시작 실패({ex.Message}). 인증 완료 후 리다이렉트 URL을 아래에 직접 붙여넣어 파싱하세요.";
            }
        }

        /// <summary>
        /// 로컬 HttpListener에서 OAuth 콜백 1건을 비동기로 수신합니다.
        /// </summary>
        private async Task ListenCallbackOnceAsync(HttpListener listener, CancellationToken cancellationToken)
        {
            try
            {
                HttpListenerContext context = await listener.GetContextAsync().ConfigureAwait(false);
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                string fullUrl = context.Request.Url != null ? context.Request.Url.ToString() : string.Empty;
                string requestBody = string.Empty;
                if (context.Request.HasEntityBody)
                {
                    using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    {
                        requestBody = await reader.ReadToEndAsync().ConfigureAwait(false);
                    }
                }

                // 브라우저 탭에 안내 HTML 응답 반환
                string html = "<!DOCTYPE html><html><head><meta charset='utf-8'><title>LifeSmart 인증 완료</title></head>"
                            + "<body style='font-family:sans-serif;padding:24px;'>"
                            + "<h2>LifeSmart 사용자 인증 콜백을 수신했습니다.</h2>"
                            + "<p>이 브라우저 창을 닫고 <b>cloud-api-sample</b> 프로그램으로 돌아가 [메인 화면에 적용]을 클릭하세요.</p>"
                            + "</body></html>";
                byte[] buffer = Encoding.UTF8.GetBytes(html);
                context.Response.ContentType = "text/html; charset=utf-8";
                context.Response.ContentLength64 = buffer.Length;
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                context.Response.OutputStream.Close();

                string captured = !string.IsNullOrWhiteSpace(requestBody) ? requestBody : fullUrl;
                BeginInvoke(new Action(() =>
                {
                    txtCallbackRaw.Text = captured;
                    ParseCallbackInput(captured);
                }));
            }
            catch (ObjectDisposedException)
            {
                // 정상 종료
            }
            catch (HttpListenerException)
            {
                // 리스너 중지
            }
            catch (Exception ex)
            {
                if (!IsDisposed && IsHandleCreated)
                {
                    BeginInvoke(new Action(() =>
                    {
                        lblStatus.Text = $"상태: 콜백 수신 중 오류: {ex.Message}";
                    }));
                }
            }
        }

        private void StopCallbackListener()
        {
            CancellationTokenSource cts = _listenerCts;
            HttpListener listener = _callbackListener;
            _listenerCts = null;
            _callbackListener = null;

            try { cts?.Cancel(); } catch { }
            try { listener?.Stop(); } catch { }
            try { listener?.Close(); } catch { }
            try { cts?.Dispose(); } catch { }
        }

        private void btnParseCallback_Click(object sender, EventArgs e)
        {
            ParseCallbackInput(txtCallbackRaw.Text);
        }

        /// <summary>
        /// 콜백 URL 쿼리스트링 또는 JSON 응답 문자열에서 userid, usertoken, svrurl, svrrgnid 등을 추출합니다.
        /// </summary>
        private void ParseCallbackInput(string rawInput)
        {
            if (string.IsNullOrWhiteSpace(rawInput))
            {
                MessageBox.Show("파싱할 콜백 URL 또는 JSON 응답이 비어 있습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string trimmed = rawInput.Trim();
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // 1) JSON 형태 응답 파싱 시도 (매뉴얼 3.3 예시 형식)
            if (trimmed.StartsWith("{", StringComparison.Ordinal))
            {
                try
                {
                    JObject obj = JObject.Parse(trimmed);
                    foreach (JProperty prop in obj.Properties())
                    {
                        values[prop.Name] = prop.Value != null ? prop.Value.ToString() : string.Empty;
                    }
                }
                catch (JsonException ex)
                {
                    lblStatus.Text = $"상태: JSON 파싱 오류 - {ex.Message}";
                    return;
                }
            }
            else
            {
                // 2) URL 쿼리스트링 또는 key=value&... 문자열 파싱
                string query = trimmed;
                int qIndex = trimmed.IndexOf('?');
                if (qIndex >= 0 && qIndex < trimmed.Length - 1)
                {
                    query = trimmed.Substring(qIndex + 1);
                }

                string[] pairs = query.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string pair in pairs)
                {
                    int eq = pair.IndexOf('=');
                    if (eq > 0)
                    {
                        string key = Uri.UnescapeDataString(pair.Substring(0, eq).Replace('+', ' '));
                        string val = Uri.UnescapeDataString(pair.Substring(eq + 1).Replace('+', ' '));
                        values[key] = val;
                    }
                }

                // 콜백 파라미터 자체가 JSON 문자열(예: data={...} 또는 res={...})로 감싸진 경우도 보조 파싱합니다.
                foreach (string candidateKey in new[] { "data", "result", "res", "ret" })
                {
                    if (values.TryGetValue(candidateKey, out string embedded) &&
                        !string.IsNullOrWhiteSpace(embedded) &&
                        embedded.TrimStart().StartsWith("{", StringComparison.Ordinal))
                    {
                        try
                        {
                            JObject nested = JObject.Parse(embedded);
                            foreach (JProperty prop in nested.Properties())
                            {
                                values[prop.Name] = prop.Value != null ? prop.Value.ToString() : string.Empty;
                            }
                        }
                        catch
                        {
                            // 무시
                        }
                    }
                }
            }

            values.TryGetValue("userid", out string userId);
            values.TryGetValue("usertoken", out string userToken);
            values.TryGetValue("svrurl", out string svrUrl);
            values.TryGetValue("svrrgnid", out string svrRgnId);
            values.TryGetValue("expiredtime", out string expiredTime);

            txtParsedUserId.Text = userId ?? string.Empty;
            txtParsedUserToken.Text = userToken ?? string.Empty;
            txtParsedSvrUrl.Text = svrUrl ?? string.Empty;
            txtParsedRegionId.Text = svrRgnId ?? string.Empty;
            ResultExpiredTime = expiredTime ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(userId) || !string.IsNullOrWhiteSpace(userToken))
            {
                lblStatus.Text = "상태: 인증 정보를 추출했습니다. [메인 화면에 적용]을 눌러 2단계에 반영하세요.";
            }
            else if (values.TryGetValue("message", out string errMsg))
            {
                lblStatus.Text = $"상태: 인증 오류 응답 - {errMsg}";
            }
            else
            {
                lblStatus.Text = "상태: userid / usertoken 항목을 찾지 못했습니다. 콜백 원문을 확인하세요.";
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ResultUserId = txtParsedUserId.Text.Trim();
            ResultUserToken = txtParsedUserToken.Text.Trim();
            ResultServerUrl = txtParsedSvrUrl.Text.Trim();
            ResultRegionId = txtParsedRegionId.Text.Trim();

            if (string.IsNullOrEmpty(ResultUserId) && string.IsNullOrEmpty(ResultUserToken))
            {
                MessageBox.Show("적용할 User ID 또는 UserToken이 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!string.IsNullOrEmpty(ResultUserId) && !ResultUserId.All(char.IsDigit))
            {
                MessageBox.Show("User ID는 숫자만 입력할 수 있습니다.", "입력 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
