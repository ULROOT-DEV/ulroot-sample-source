using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using LocalUdpSample.Protocol;

namespace LocalUdpSample.Client
{
    /// <summary>
    /// Smart Station으로부터 실시간 로컬 이벤트(pkg_type = 9, NOTIFY)를 수신할 때 전달되는 이벤트 인자입니다.
    /// </summary>
    public sealed class LifeSmartEventReceivedArgs : EventArgs
    {
        /// <summary>이벤트를 송신한 Smart Station의 원격 엔드포인트</summary>
        public IPEndPoint RemoteEndPoint { get; }

        /// <summary>파싱된 LifeSmart 패킷</summary>
        public LifeSmartPacket Packet { get; }

        public LifeSmartEventReceivedArgs(IPEndPoint remoteEndPoint, LifeSmartPacket packet)
        {
            RemoteEndPoint = remoteEndPoint;
            Packet = packet;
        }
    }

    /// <summary>
    /// LifeSmart Smart Station의 로컬 이벤트 서비스(Local Event Service)를 수신하는 비동기 UDP 리스너입니다.
    /// (매뉴얼 4. Local Event Service 참조)
    ///
    /// [사전 조건]
    /// 1. 이 리스너를 시작(Start)하여 지정된 UDP 포트를 바인딩합니다.
    /// 2. LifeSmartUdpClient.RegisterNotifyAsync()로 Smart Station에 본 PC의 IP와 수신 포트를 등록합니다.
    /// 3. 등록은 300초 뒤 만료되므로, 300초 이내(권장: 240초 주기)마다 재등록해야 합니다.
    /// 4. Windows 방화벽에서 수신용 UDP 포트 인바운드가 허용되어 있어야 합니다.
    /// </summary>
    public sealed class LifeSmartEventReceiver : IDisposable
    {
        private UdpClient _udpClient;
        private CancellationTokenSource _cts;
        private Task _receiveLoopTask;

        /// <summary>현재 수신 대기 중인 로컬 UDP 포트 번호</summary>
        public int ListenPort { get; private set; }

        /// <summary>현재 이벤트 수신 리스너가 실행 중인지 여부</summary>
        public bool IsRunning => _udpClient != null;

        /// <summary>유효한 LifeSmart 이벤트 패킷을 수신했을 때 발생합니다. (백그라운드 스레드에서 호출됨)</summary>
        public event EventHandler<LifeSmartEventReceivedArgs> EventReceived;

        /// <summary>수신 또는 파싱 중 오류가 발생했을 때 발생합니다. (백그라운드 스레드에서 호출됨)</summary>
        public event EventHandler<string> ErrorOccurred;

        /// <summary>
        /// 지정한 UDP 포트에서 실시간 이벤트 수신을 시작합니다.
        /// </summary>
        /// <param name="port">수신 대기할 로컬 UDP 포트 번호 (예: 12350)</param>
        public void Start(int port)
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("이벤트 수신기가 이미 실행 중입니다. 먼저 Stop()을 호출하세요.");
            }

            if (port < 1 || port > 65535)
            {
                throw new ArgumentOutOfRangeException(nameof(port), "포트 번호는 1~65535 범위여야 합니다.");
            }

            var udpClient = new UdpClient(AddressFamily.InterNetwork);
            try
            {
                // 포트 재사용 옵션을 Bind 이전에 설정합니다.
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.ExclusiveAddressUse = false;
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));
            }
            catch
            {
                udpClient.Dispose();
                throw;
            }

            _udpClient = udpClient;
            ListenPort = port;
            _cts = new CancellationTokenSource();
            _receiveLoopTask = Task.Run(() => ReceiveLoopAsync(_udpClient, _cts.Token));
        }

        /// <summary>
        /// 이벤트 수신을 중지하고 UDP 소켓 리소스를 안전하게 해제합니다.
        /// </summary>
        public void Stop()
        {
            CancellationTokenSource cts = _cts;
            UdpClient udpClient = _udpClient;

            _cts = null;
            _udpClient = null;
            _receiveLoopTask = null;

            try
            {
                cts?.Cancel();
            }
            catch
            {
                // 종료 시 예외 무시
            }

            try
            {
                // 소켓을 닫으면 대기 중인 ReceiveAsync()가 즉시 해제됩니다.
                udpClient?.Close();
                udpClient?.Dispose();
            }
            catch
            {
                // 종료 시 예외 무시
            }
            finally
            {
                cts?.Dispose();
            }
        }

        /// <summary>
        /// 백그라운드에서 UDP 패킷을 지속적으로 수신하고 파싱하는 비동기 루프입니다.
        /// 단일 패킷의 형식이 잘못되더라도 루프가 중단되지 않도록 예외를 안전하게 처리합니다.
        /// </summary>
        private async Task ReceiveLoopAsync(UdpClient udpClient, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    UdpReceiveResult result = await udpClient.ReceiveAsync().ConfigureAwait(false);

                    if (LifeSmartPacket.TryParse(result.Buffer, out LifeSmartPacket packet, out string errorReason))
                    {
                        EventReceived?.Invoke(this, new LifeSmartEventReceivedArgs(result.RemoteEndPoint, packet));
                    }
                    else
                    {
                        ErrorOccurred?.Invoke(
                            this,
                            $"[{result.RemoteEndPoint}] 이벤트 패킷 파싱 실패: {errorReason}");
                    }
                }
                catch (ObjectDisposedException)
                {
                    // Stop() 호출로 소켓이 닫힌 정상 종료 상황
                    break;
                }
                catch (SocketException ex)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    ErrorOccurred?.Invoke(this, $"UDP 수신 소켓 오류 (Code={ex.SocketErrorCode}): {ex.Message}");
                }
                catch (Exception ex)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    ErrorOccurred?.Invoke(this, $"UDP 이벤트 수신 중 예기치 않은 오류: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
