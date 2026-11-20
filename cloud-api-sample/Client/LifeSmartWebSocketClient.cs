using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CloudApiSample.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudApiSample.Client
{
    /// <summary>
    /// LifeSmart Cloud Platform WebSocket 실시간 이벤트 수신 클라이언트입니다.
    /// (매뉴얼 7. WebSocket Event 참조)
    ///
    /// [통신 흐름 - 매뉴얼 7.1장]
    /// 1. 계정 리전(svrrgnid)에 맞는 WSS URL로 연결 (예: wss://api.us.ilifesmart.com:8443/wsapp/)
    /// 2. 연결 직후 즉시 WbAuth JSON 메시지 전송 (미인증 상태로 오래 두면 서버가 연결을 종료함)
    /// 3. 서버로부터 인증 응답 및 실시간 이벤트(type="io" 등), 하트비트 메시지 수신
    /// 4. 종료 시 RmAuth 메시지를 전송해 인증을 해제한 뒤 정상 Close 수행
    /// </summary>
    public sealed class LifeSmartWebSocketClient : IDisposable
    {
        private ClientWebSocket _webSocket;
        private CancellationTokenSource _cts;
        private Task _receiveLoopTask;
        private CloudApiConfig _connectedConfig;
        private int _sequenceId;

        /// <summary>현재 WebSocket이 열려 있는지 여부</summary>
        public bool IsConnected => _webSocket != null && _webSocket.State == WebSocketState.Open;

        /// <summary>서버로부터 텍스트/JSON 메시지를 수신했을 때 발생합니다. (백그라운드 스레드에서 호출됨)</summary>
        public event EventHandler<string> MessageReceived;

        /// <summary>연결 상태 변경 또는 진단 로그 발생 시 호출됩니다.</summary>
        public event EventHandler<string> StatusChanged;

        /// <summary>WebSocket 연결이 종료되었을 때 발생합니다.</summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// WebSocket 서버에 연결한 직후 WbAuth 인증 메시지를 전송하고 실시간 수신 루프를 시작합니다.
        /// </summary>
        public async Task ConnectAndAuthenticateAsync(
            CloudApiConfig config,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            config.EnsureUserCredentials();

            if (string.IsNullOrWhiteSpace(config.WebSocketUrl))
            {
                throw new InvalidOperationException("WebSocket URL을 입력하세요.");
            }

            if (IsConnected)
            {
                throw new InvalidOperationException("WebSocket이 이미 연결되어 있습니다.");
            }

            var ws = new ClientWebSocket();
            // 서버와의 연결 유지를 위한 WebSocket Keep-Alive 주기 설정
            ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(30);

            try
            {
                var uri = new Uri(config.WebSocketUrl);
                StatusChanged?.Invoke(this, $"[WS] 서버 연결 시도 중: {uri}");

                await ws.ConnectAsync(uri, cancellationToken).ConfigureAwait(false);
                StatusChanged?.Invoke(this, "[WS] WebSocket 연결 성공. WbAuth 인증 메시지를 전송합니다...");

                _webSocket = ws;
                _connectedConfig = config;
                _cts = new CancellationTokenSource();

                // 매뉴얼 7.3: 연결 직후 즉시 WbAuth 인증 메시지를 전송합니다.
                JObject authPayload = BuildAuthPayload(config, "WbAuth");
                await SendTextAsync(ws, authPayload.ToString(Formatting.None), cancellationToken).ConfigureAwait(false);
                StatusChanged?.Invoke(this, $"[WS -> SEND WbAuth]\r\n{authPayload.ToString(Formatting.Indented)}");

                // 백그라운드 수신 루프 시작
                _receiveLoopTask = Task.Run(() => ReceiveLoopAsync(ws, _cts.Token));
            }
            catch
            {
                ws.Dispose();
                _webSocket = null;
                _connectedConfig = null;
                _cts?.Dispose();
                _cts = null;
                throw;
            }
        }

        /// <summary>
        /// 매뉴얼 7.4에 따라 RmAuth 인증 해제 메시지를 전송한 뒤 WebSocket 연결을 안전하게 종료합니다.
        /// </summary>
        public async Task DisconnectAsync()
        {
            ClientWebSocket ws = _webSocket;
            CancellationTokenSource cts = _cts;
            CloudApiConfig config = _connectedConfig;

            _webSocket = null;
            _cts = null;
            _connectedConfig = null;
            _receiveLoopTask = null;

            if (ws == null)
            {
                return;
            }

            try
            {
                if (ws.State == WebSocketState.Open)
                {
                    // 1) RmAuth 전송 (매뉴얼 7.4: 연결된 사용자가 0명이 되면 서버도 연결 종료)
                    if (config != null)
                    {
                        try
                        {
                            using (var sendCts = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
                            {
                                JObject rmAuthPayload = BuildAuthPayload(config, "RmAuth");
                                await SendTextAsync(ws, rmAuthPayload.ToString(Formatting.None), sendCts.Token)
                                    .ConfigureAwait(false);
                                StatusChanged?.Invoke(this, "[WS -> SEND RmAuth] 인증 해제 요청 전송 완료");
                            }
                        }
                        catch
                        {
                            // 종료 중 RmAuth 실패는 무시하고 소켓 닫기 진행
                        }
                    }

                    // 2) 정상 Close 핸드셰이크 수행
                    try
                    {
                        using (var closeCts = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
                        {
                            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnect", closeCts.Token)
                                .ConfigureAwait(false);
                        }
                    }
                    catch
                    {
                        // 원격 종료 등으로 인한 예외 무시
                    }
                }
            }
            finally
            {
                try { cts?.Cancel(); } catch { }
                try { ws.Dispose(); } catch { }
                try { cts?.Dispose(); } catch { }
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// WbAuth 또는 RmAuth 요청 JSON을 생성하고 MD5 서명을 부여합니다.
        /// </summary>
        private JObject BuildAuthPayload(CloudApiConfig config, string method)
        {
            int messageId = Interlocked.Increment(ref _sequenceId);
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string sign = CloudApiSigner.CreateOpenApiSignature(method, null, config, timestamp);

            var systemObj = new JObject
            {
                ["ver"] = "1.0",
                ["lang"] = config.Language,
                ["userid"] = config.UserId,
                ["appkey"] = config.AppKey,
                ["time"] = timestamp,
                ["sign"] = sign
            };

            if (!string.IsNullOrWhiteSpace(config.Did))
            {
                systemObj["did"] = config.Did;
            }

            return new JObject
            {
                ["id"] = messageId,
                ["method"] = method,
                ["system"] = systemObj
            };
        }

        /// <summary>
        /// UTF-8 텍스트 프레임을 WebSocket으로 전송합니다.
        /// </summary>
        private static Task SendTextAsync(ClientWebSocket ws, string text, CancellationToken cancellationToken)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            var segment = new ArraySegment<byte>(bytes);
            return ws.SendAsync(segment, WebSocketMessageType.Text, endOfMessage: true, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 백그라운드에서 WebSocket 메시지를 지속적으로 수신합니다.
        /// 큰 메시지가 여러 프레임으로 분할되어 도착(EndOfMessage == false)하더라도 끝까지 조립해 전달합니다.
        /// </summary>
        private async Task ReceiveLoopAsync(ClientWebSocket ws, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[8192];

            try
            {
                while (!cancellationToken.IsCancellationRequested && ws.State == WebSocketState.Open)
                {
                    using (var ms = new MemoryStream())
                    {
                        WebSocketReceiveResult result;
                        do
                        {
                            result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken)
                                .ConfigureAwait(false);

                            if (result.MessageType == WebSocketMessageType.Close)
                            {
                                StatusChanged?.Invoke(
                                    this,
                                    $"[WS] 서버가 연결을 종료했습니다. (Status={result.CloseStatus}, Description={result.CloseStatusDescription})");
                                return;
                            }

                            if (result.Count > 0)
                            {
                                ms.Write(buffer, 0, result.Count);
                            }
                        }
                        while (!result.EndOfMessage);

                        string rawText = Encoding.UTF8.GetString(ms.ToArray());
                        string formatted = FormatJsonOrRaw(rawText);
                        MessageReceived?.Invoke(this, formatted);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // 사용자가 정상적으로 연결을 해제한 경우
            }
            catch (ObjectDisposedException)
            {
                // 종료 과정에서 소켓이 해제된 경우
            }
            catch (WebSocketException ex)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    StatusChanged?.Invoke(this, $"[WS 오류] WebSocket 통신 예외: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    StatusChanged?.Invoke(this, $"[WS 오류] 수신 루프 예외: {ex.Message}");
                }
            }
            finally
            {
                // 원격 끊김 등으로 루프가 빠져나왔을 때 내부 참조 정리
                if (ReferenceEquals(_webSocket, ws))
                {
                    _webSocket = null;
                    _connectedConfig = null;
                    try { ws.Dispose(); } catch { }
                    Disconnected?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// 수신 문자열이 JSON이면 보기 좋게 들여쓰기하여 반환하고, 아니면 원문을 반환합니다.
        /// </summary>
        private static string FormatJsonOrRaw(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            try
            {
                return JToken.Parse(text).ToString(Formatting.Indented);
            }
            catch (JsonException)
            {
                return text;
            }
        }

        public void Dispose()
        {
            CancellationTokenSource cts = _cts;
            ClientWebSocket ws = _webSocket;
            _cts = null;
            _webSocket = null;
            _connectedConfig = null;

            try { cts?.Cancel(); } catch { }
            try { ws?.Dispose(); } catch { }
            try { cts?.Dispose(); } catch { }
        }
    }
}
