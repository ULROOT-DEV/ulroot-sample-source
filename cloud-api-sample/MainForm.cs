using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CloudApiSample.Client;
using CloudApiSample.Protocol;
using Newtonsoft.Json.Linq;

namespace CloudApiSample
{
    /// <summary>
    /// LifeSmart Cloud Platform OpenAPI & WebSocket 샘플 애플리케이션 메인 화면입니다.
    ///
    /// [진행 순서]
    /// - 공통 필수: AppKey, AppToken 입력 (모든 인증·OpenAPI·WebSocket 서명에 사용)
    /// - 1단계 (선택): UserToken이 없는 경우에만 [3.1 인증 URL 팝업]으로 UserID/UserToken 발급
    /// - 2단계: 기존 UserToken이 있다면 1단계 없이 UserID(숫자), UserToken, Region/URL을 바로 입력해 OpenAPI 및 WebSocket 사용
    ///
    /// [구현된 API]
    /// - 3.1    auth.authorize 팝업 (OAuth 인증 URL 생성 및 콜백 자동 수신)
    /// - 3.4    auth.refreshtoken   (UserToken 갱신)
    /// - 4.5.6  EpGetAll            (전체 디바이스 정보 조회)
    /// - 4.5.7  EpGet               (단일 디바이스 정보 조회)
    /// - 4.5.8  EpSet               (단일 디바이스 제어)
    /// - 4.5.10 SceneGet            (씬 목록 조회)
    /// - 4.5.11 SceneSet            (씬 실행)
    /// - 7장    WebSocket Event     (WSS 접속 + WbAuth 인증 + 실시간 이벤트 수신 + RmAuth 해제)
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>로그 창 최대 문자 수 (장시간 WebSocket 이벤트 수신 시 메모리 누적 방지)</summary>
        private const int MaxLogCharacters = 150000;

        private readonly LifeSmartCloudClient _cloudClient = new LifeSmartCloudClient();
        private readonly LifeSmartWebSocketClient _wsClient = new LifeSmartWebSocketClient();

        public MainForm()
        {
            InitializeComponent();

            _cloudClient.OnDebugLog = AppendLog;
            _wsClient.MessageReceived += WsClient_MessageReceived;
            _wsClient.StatusChanged += WsClient_StatusChanged;
            _wsClient.Disconnected += WsClient_Disconnected;

            // 리전 목록 초기화 (한국 계정 기본값 AME 우선 선택)
            foreach (CloudRegionInfo region in CloudRegionInfo.AllRegions)
            {
                cboRegion.Items.Add(region);
            }
            cboRegion.SelectedIndex = 0;

            // EpGetAll degree 선택 옵션 초기화
            cboDegree.Items.AddRange(new object[]
            {
                "기본 (생략)",
                "0 - 기본 정보",
                "1 - data 포함",
                "2 - 상세 정보",
                "3 - agt_self 포함"
            });
            cboDegree.SelectedIndex = 0;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            AppendLog("LifeSmart Cloud API 샘플이 준비되었습니다.");
            AppendLog("[공통 필수] AppKey, AppToken을 입력하세요.");
            AppendLog("[1단계 (선택)] UserToken이 아직 없는 경우에만 [3.1 인증 URL 팝업]으로 사용자 로그인·승인을 진행해 UserToken을 발급받으세요.");
            AppendLog("[2단계] 이미 UserToken을 보유하고 있다면 1단계 없이 UserID(숫자)·UserToken만 입력하면 바로 OpenAPI 및 WebSocket을 사용할 수 있습니다.");
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 종료 시 WebSocket과 HttpClient 리소스를 안전하게 정리합니다.
            if (_wsClient.IsConnected)
            {
                try
                {
                    await _wsClient.DisconnectAsync();
                }
                catch
                {
                    // 종료 중 예외 무시
                }
            }

            _wsClient.Dispose();
            _cloudClient.Dispose();
        }

        #region 1단계/2단계 인증 및 서버 설정

        private void txtUserId_KeyPress(object sender, KeyPressEventArgs e)
        {
            // UserID는 숫자만 입력할 수 있도록 제한합니다 (백스페이스, 복사/붙여넣기 등 제어 키는 허용).
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cboRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRegion.SelectedItem is CloudRegionInfo region)
            {
                txtServerUrl.Text = region.OpenApiBaseUrl;
                txtWsUrl.Text = region.WebSocketUrl;
            }
        }

        private void btnOpenAuthPopup_Click(object sender, EventArgs e)
        {
            string appKey = txtAppKey.Text.Trim();
            string appToken = txtAppToken.Text.Trim();
            if (string.IsNullOrEmpty(appKey) || string.IsNullOrEmpty(appToken))
            {
                MessageBox.Show(
                    "[기본 앱 설정] 인증 URL 서명 생성을 위해 AppKey와 AppToken을 먼저 입력하세요.",
                    "입력 확인",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using (var popup = new AuthPopupForm(txtServerUrl.Text.Trim(), appKey, appToken))
            {
                if (popup.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(popup.ResultUserId))
                {
                    txtUserId.Text = popup.ResultUserId;
                }

                if (!string.IsNullOrEmpty(popup.ResultUserToken))
                {
                    txtUserToken.Text = popup.ResultUserToken;
                }

                // 인증 응답의 svrrgnid 또는 svrurl에 맞춰 2단계 리전/URL을 자동 반영합니다.
                CloudRegionInfo matchedRegion = CloudRegionInfo.FindByRegionId(popup.ResultRegionId)
                                             ?? CloudRegionInfo.FindByOpenApiUrl(popup.ResultServerUrl);
                if (matchedRegion != null)
                {
                    cboRegion.SelectedItem = matchedRegion;
                }

                if (!string.IsNullOrEmpty(popup.ResultServerUrl))
                {
                    txtServerUrl.Text = CloudRegionInfo.NormalizeBaseUrl(popup.ResultServerUrl);
                }

                AppendLog($"[1단계 인증 완료 -> 2단계 반영] userid={txtUserId.Text}, region={popup.ResultRegionId}, svrurl={txtServerUrl.Text}");
            }
        }

        private async void btnRefreshToken_Click(object sender, EventArgs e)
        {
            await ExecuteCloudApiAsync(
                btnRefreshToken,
                "3.4 UserToken 갱신 (auth.refreshtoken)",
                config => _cloudClient.RefreshUserTokenAsync(config),
                response =>
                {
                    // 갱신 성공 시 새로 발급된 usertoken을 2단계 입력란에 자동 반영합니다.
                    string newToken = response.Payload?["usertoken"]?.Value<string>();
                    if (response.Code == 0 && !string.IsNullOrWhiteSpace(newToken))
                    {
                        txtUserToken.Text = newToken;
                        AppendLog("[안내] 새로 발급된 UserToken을 입력란에 자동 반영했습니다.");
                    }
                });
        }

        #endregion

        #region 4.5.6 ~ 4.5.8 디바이스 API (EpGetAll / EpGet / EpSet)

        private async void btnEpGetAll_Click(object sender, EventArgs e)
        {
            // 콤보박스 0번은 "기본 (생략)", 1~4번은 degree 0~3에 해당합니다.
            int? degree = cboDegree.SelectedIndex > 0 ? (int?)(cboDegree.SelectedIndex - 1) : null;

            await ExecuteCloudApiAsync(
                btnEpGetAll,
                degree.HasValue ? $"4.5.6 전체 디바이스 조회 (EpGetAll, degree={degree.Value})" : "4.5.6 전체 디바이스 조회 (EpGetAll)",
                config => _cloudClient.EpGetAllAsync(config, degree),
                response =>
                {
                    // 조회 성공 시 AGT / ME 입력란이 비어 있으면 첫 번째 디바이스 값을 편의상 자동 채워줍니다.
                    if (response.Code == 0 && response.Payload?["message"] is JArray devices && devices.Count > 0)
                    {
                        var first = devices[0] as JObject;
                        string firstAgt = first?["agt"]?.Value<string>();
                        string firstMe = first?["me"]?.Value<string>();

                        if (string.IsNullOrWhiteSpace(txtAgt.Text) && !string.IsNullOrWhiteSpace(firstAgt))
                        {
                            txtAgt.Text = firstAgt;
                        }
                        if (string.IsNullOrWhiteSpace(txtMe.Text) && !string.IsNullOrWhiteSpace(firstMe))
                        {
                            txtMe.Text = firstMe;
                        }
                    }
                });
        }

        private async void btnEpGet_Click(object sender, EventArgs e)
        {
            string agt = txtAgt.Text.Trim();
            string me = txtMe.Text.Trim();
            if (string.IsNullOrEmpty(agt) || string.IsNullOrEmpty(me))
            {
                MessageBox.Show(
                    "AGT(Smart Station ID)와 ME(Device ID)를 입력하세요.\n먼저 [EpGetAll]을 호출하면 값을 확인할 수 있습니다.",
                    "입력 확인",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            await ExecuteCloudApiAsync(
                btnEpGet,
                $"4.5.7 단일 디바이스 조회 (EpGet: agt={agt}, me={me})",
                config => _cloudClient.EpGetAsync(config, agt, me));
        }

        private void btnEpPresetOn_Click(object sender, EventArgs e)
        {
            // 일반 스위치/소켓 ON 명령: type = 0x81 (129), val = 1
            txtEpType.Text = "129";
            txtEpVal.Text = "1";
        }

        private void btnEpPresetOff_Click(object sender, EventArgs e)
        {
            // 일반 스위치/소켓 OFF 명령: type = 0x80 (128), val = 0
            txtEpType.Text = "128";
            txtEpVal.Text = "0";
        }

        private async void btnEpSet_Click(object sender, EventArgs e)
        {
            string agt = txtAgt.Text.Trim();
            string me = txtMe.Text.Trim();
            string idx = txtIdx.Text.Trim();
            string tag = txtEpTag.Text.Trim();

            if (string.IsNullOrEmpty(agt) || string.IsNullOrEmpty(me) || string.IsNullOrEmpty(idx))
            {
                MessageBox.Show(
                    "AGT, ME, IO(idx)를 입력하세요.\n예: idx=P1, L1 (스위치), O (소켓), RGBW (조명)",
                    "입력 확인",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (!int.TryParse(txtEpType.Text.Trim(), out int controlType) ||
                !int.TryParse(txtEpVal.Text.Trim(), out int controlVal))
            {
                MessageBox.Show(
                    "Type과 Val에는 정수를 입력하세요.\n예: ON은 type=129, val=1 / OFF는 type=128, val=0",
                    "입력 확인",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            await ExecuteCloudApiAsync(
                btnEpSet,
                $"4.5.8 디바이스 제어 (EpSet: agt={agt}, me={me}, idx={idx}, type={controlType}, val={controlVal})",
                config => _cloudClient.EpSetAsync(config, agt, me, idx, controlType, controlVal, tag));
        }

        #endregion

        #region 4.5.10 ~ 4.5.11 씬 API (SceneGet / SceneSet)

        private async void btnSceneGet_Click(object sender, EventArgs e)
        {
            string agt = txtAgt.Text.Trim();
            if (string.IsNullOrEmpty(agt))
            {
                MessageBox.Show(
                    "상단 디바이스 영역의 AGT(Smart Station ID)를 입력하세요.\n먼저 [EpGetAll]을 호출하면 AGT를 확인할 수 있습니다.",
                    "입력 확인",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            await ExecuteCloudApiAsync(
                btnSceneGet,
                $"4.5.10 씬 목록 조회 (SceneGet: agt={agt})",
                config => _cloudClient.SceneGetAsync(config, agt),
                response =>
                {
                    // 씬 목록 조회 성공 시 첫 번째 씬 ID를 편의상 자동 채워줍니다.
                    if (response.Code == 0 && response.Payload?["message"] is JArray scenes && scenes.Count > 0)
                    {
                        string firstSceneId = (scenes[0] as JObject)?["id"]?.Value<string>();
                        if (string.IsNullOrWhiteSpace(txtSceneId.Text) && !string.IsNullOrWhiteSpace(firstSceneId))
                        {
                            txtSceneId.Text = firstSceneId;
                        }
                    }
                });
        }

        private async void btnSceneSet_Click(object sender, EventArgs e)
        {
            string agt = txtAgt.Text.Trim();
            string sceneId = txtSceneId.Text.Trim();
            if (string.IsNullOrEmpty(agt) || string.IsNullOrEmpty(sceneId))
            {
                MessageBox.Show(
                    "AGT(Smart Station ID)와 Scene ID를 입력하세요.\n먼저 [SceneGet]을 호출하면 Scene ID를 확인할 수 있습니다.",
                    "입력 확인",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            int? sceneType = null;
            if (!string.IsNullOrWhiteSpace(txtSceneType.Text))
            {
                if (!int.TryParse(txtSceneType.Text.Trim(), out int parsedType))
                {
                    MessageBox.Show("Scene Type은 정수로 입력하거나 비워두세요.", "입력 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                sceneType = parsedType;
            }

            int? sceneRgbw = null;
            if (!string.IsNullOrWhiteSpace(txtSceneRgbw.Text))
            {
                if (!int.TryParse(txtSceneRgbw.Text.Trim(), out int parsedRgbw))
                {
                    MessageBox.Show("Scene RGBW는 정수로 입력하거나 비워두세요.", "입력 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                sceneRgbw = parsedRgbw;
            }

            await ExecuteCloudApiAsync(
                btnSceneSet,
                $"4.5.11 씬 실행 (SceneSet: agt={agt}, id={sceneId})",
                config => _cloudClient.SceneSetAsync(config, agt, sceneId, sceneType, sceneRgbw));
        }

        #endregion

        #region 7. WebSocket Event 실시간 수신

        private async void btnWsToggle_Click(object sender, EventArgs e)
        {
            if (_wsClient.IsConnected)
            {
                btnWsToggle.Enabled = false;
                try
                {
                    await _wsClient.DisconnectAsync();
                }
                finally
                {
                    btnWsToggle.Enabled = true;
                }
                return;
            }

            if (!TryBuildCloudConfig(out CloudApiConfig config))
            {
                return;
            }

            btnWsToggle.Enabled = false;
            try
            {
                await _wsClient.ConnectAndAuthenticateAsync(config);
                SetWebSocketUiState(isConnected: true);
            }
            catch (Exception ex)
            {
                AppendLog($"[WS 오류] 연결 또는 인증 실패: {ex.Message}");
                MessageBox.Show($"WebSocket 연결에 실패했습니다.\n{ex.Message}", "WebSocket 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetWebSocketUiState(isConnected: false);
            }
            finally
            {
                btnWsToggle.Enabled = true;
            }
        }

        private void WsClient_MessageReceived(object sender, string message)
        {
            AppendLog($"[WS <- EVENT]\r\n{message}");
        }

        private void WsClient_StatusChanged(object sender, string statusMessage)
        {
            AppendLog(statusMessage);
        }

        private void WsClient_Disconnected(object sender, EventArgs e)
        {
            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }

            BeginInvoke(new Action(() =>
            {
                SetWebSocketUiState(isConnected: false);
                AppendLog("[WS] WebSocket 연결이 종료되었습니다.");
            }));
        }

        private void SetWebSocketUiState(bool isConnected)
        {
            btnWsToggle.Text = isConnected ? "WebSocket 연결 해제 (RmAuth)" : "WebSocket 연결 + WbAuth";
            if (isConnected)
            {
                lblWsStatus.Text = "상태: 연결됨 (실시간 이벤트 수신 대기 중)";
                lblWsStatus.ForeColor = Color.DarkGreen;
            }
            else
            {
                lblWsStatus.Text = "상태: 연결 안 됨 (연결 시 자동 WbAuth 인증)";
                lblWsStatus.ForeColor = Color.DimGray;
            }
        }

        #endregion

        #region 공통 헬퍼 메서드

        /// <summary>
        /// 공통 비동기 OpenAPI 실행 래퍼입니다.
        /// 요청 중 버튼을 비활성화하고, 응답 JSON 또는 오류 메시지를 로그 창에 출력합니다.
        /// </summary>
        private async Task ExecuteCloudApiAsync(
            Button triggerButton,
            string actionDescription,
            Func<CloudApiConfig, Task<CloudApiResponse>> requestFunc,
            Action<CloudApiResponse> onSuccess = null)
        {
            if (!TryBuildCloudConfig(out CloudApiConfig config))
            {
                return;
            }

            triggerButton.Enabled = false;
            try
            {
                AppendLog($"요청 실행: {actionDescription}");
                CloudApiResponse response = await requestFunc(config);
                AppendLog($"[응답 수신 (HTTP {response.HttpStatusCode})]\r\n{response.ToPrettyJson()}");
                onSuccess?.Invoke(response);
            }
            catch (Exception ex)
            {
                AppendLog($"[오류] {actionDescription} 실패: {ex.Message}");
            }
            finally
            {
                triggerButton.Enabled = true;
            }
        }

        /// <summary>
        /// 1단계/2단계 입력란의 Cloud API 인증·서버 정보를 검증하고 설정 객체를 생성합니다.
        /// </summary>
        private bool TryBuildCloudConfig(out CloudApiConfig config)
        {
            config = null;

            string appKey = txtAppKey.Text.Trim();
            string appToken = txtAppToken.Text.Trim();
            string userId = txtUserId.Text.Trim();
            string userToken = txtUserToken.Text.Trim();
            string serverUrl = txtServerUrl.Text.Trim();
            string wsUrl = txtWsUrl.Text.Trim();

            if (string.IsNullOrEmpty(appKey) || string.IsNullOrEmpty(appToken))
            {
                MessageBox.Show("[기본 앱 설정] AppKey와 AppToken을 입력하세요.", "인증 정보 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userToken))
            {
                MessageBox.Show(
                    "[2단계] UserID(숫자)와 UserToken을 입력하세요.\n기존 UserToken이 있다면 1단계 없이 바로 입력해 사용할 수 있으며, 아직 없다면 [1단계: 3.1 인증 URL 팝업]에서 먼저 발급받으세요.",
                    "인증 정보 확인",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return false;
            }

            if (!userId.All(char.IsDigit))
            {
                MessageBox.Show(
                    "[2단계] UserID(user_no)는 숫자만 입력할 수 있습니다.",
                    "입력 확인",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return false;
            }

            config = new CloudApiConfig(
                appKey: appKey,
                appToken: appToken,
                userId: userId,
                userToken: userToken,
                serverUrl: serverUrl,
                webSocketUrl: wsUrl);
            return true;
        }

        /// <summary>
        /// 어떤 스레드에서 호출되더라도 안전하게 로그 창에 타임스탬프와 메시지를 추가합니다.
        /// </summary>
        private void AppendLog(string message)
        {
            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action<string>(AppendLog), message);
                }
                catch (ObjectDisposedException)
                {
                    // 폼 종료 중 발생하는 비동기 호출 무시
                }
                return;
            }

            if (rtbLog.TextLength > MaxLogCharacters)
            {
                rtbLog.Clear();
            }

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            rtbLog.AppendText($"[{timestamp}] {message}{Environment.NewLine}");
            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.ScrollToCaret();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }

        #endregion
    }
}
