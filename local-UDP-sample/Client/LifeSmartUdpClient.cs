using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using LocalUdpSample.Protocol;
using Newtonsoft.Json.Linq;

namespace LocalUdpSample.Client
{
    /// <summary>
    /// LifeSmart Smart Station Local UDP API(OpenDev)와 비동기로 요청/응답 통신을 수행하는 클라이언트입니다.
    ///
    /// [지원 예시 API - 매뉴얼 3.3장 참조]
    /// - GET  eps          : 서브 디바이스 전체 목록 조회 (3.3.2)
    /// - GET  ep           : 단일 서브 디바이스 정보 조회 (3.3.1)
    /// - SET  ep           : 단일 서브 디바이스 제어 (3.3.3)
    /// - GET  scene        : 씬 목록 조회 (3.3.5)
    /// - SET  doscene      : 씬 실행 (3.3.6)
    /// - SET  config       : Smart Station 설정 명령 - notify, getver, reboot 등 (3.3.10)
    /// </summary>
    public sealed class LifeSmartUdpClient : IDisposable
    {
        /// <summary>JSON sys.ver 프로토콜 버전 (매뉴얼 3.1.1: 숫자 1 고정)</summary>
        private const int JsonProtocolVersion = 1;

        /// <summary>요청 메시지 일련번호(id) 생성용 카운터</summary>
        private int _sequenceId;

        /// <summary>
        /// 동일한 로컬 포트(예: 12346)를 사용할 때 동시 요청 간 소켓 충돌을 방지하기 위한 잠금 객체입니다.
        /// </summary>
        private readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// 요청 전송 직전에 원문 JSON이나 디버깅 로그를 확인하고 싶을 때 사용하는 콜백입니다.
        /// </summary>
        public Action<string> OnDebugLog { get; set; }

        /// <summary>
        /// Smart Station에 연결된 전체 서브 디바이스 목록을 조회합니다. (GET / obj: "eps", 매뉴얼 3.3.2)
        /// </summary>
        /// <param name="config">Smart Station 접속 정보</param>
        /// <param name="degree">
        /// 선택 파라미터(0, 1, 2).
        /// null이면 생략(기본값 2: 디바이스 상세 data 포함). 0 또는 1 지정 시 data 필드를 제외하고 경량 목록만 수신합니다.
        /// </param>
        public Task<LifeSmartPacket> GetDevicesAsync(
            LifeSmartStationConfig config,
            int? degree = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var args = new JObject();
            if (degree.HasValue)
            {
                args["degree"] = degree.Value;
            }

            return SendRequestAsync(config, LifeSmartPackageType.Get, "eps", args, cancellationToken);
        }

        /// <summary>
        /// 특정 서브 디바이스 1대의 상태를 조회합니다. (GET / obj: "ep", 매뉴얼 3.3.1)
        /// </summary>
        /// <param name="config">Smart Station 접속 정보</param>
        /// <param name="deviceId">서브 디바이스 ID (me, 예: "2711")</param>
        public Task<LifeSmartPacket> GetDeviceAsync(
            LifeSmartStationConfig config,
            string deviceId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var args = new JObject
            {
                ["me"] = deviceId
            };

            return SendRequestAsync(config, LifeSmartPackageType.Get, "ep", args, cancellationToken);
        }

        /// <summary>
        /// 단일 서브 디바이스를 제어합니다. (SET / obj: "ep", 매뉴얼 3.3.3)
        /// 예: 스위치/소켓 켜기(type=0x81(129), val=1), 끄기(type=0x80(128), val=0)
        /// </summary>
        /// <param name="config">Smart Station 접속 정보</param>
        /// <param name="deviceId">서브 디바이스 ID (me, 예: "2711")</param>
        /// <param name="ioIndex">제어할 IO 인덱스 (idx, 예: "L1", "L2", "O", "RGBW")</param>
        /// <param name="controlType">제어 타입 코드 (예: 0x81=129 ON, 0x80=128 OFF, 0xFF=255 색상+ON)</param>
        /// <param name="value">제어 값 (예: 1 또는 0)</param>
        /// <param name="tag">태그 문자열 (매뉴얼 기본 예시: "m")</param>
        public Task<LifeSmartPacket> ControlDeviceAsync(
            LifeSmartStationConfig config,
            string deviceId,
            string ioIndex,
            int controlType,
            int value,
            string tag = "m",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var args = new JObject
            {
                ["tag"] = tag,
                ["me"] = deviceId,
                ["idx"] = ioIndex,
                ["type"] = controlType,
                ["val"] = value
            };

            return SendRequestAsync(config, LifeSmartPackageType.Set, "ep", args, cancellationToken);
        }

        /// <summary>
        /// Smart Station에 등록된 씬(Scene) 목록을 조회합니다. (GET / obj: "scene", 매뉴얼 3.3.5)
        /// </summary>
        public Task<LifeSmartPacket> GetScenesAsync(
            LifeSmartStationConfig config,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return SendRequestAsync(config, LifeSmartPackageType.Get, "scene", new JObject(), cancellationToken);
        }

        /// <summary>
        /// Smart Station의 현재 펌웨어 버전 및 모델 타입을 조회합니다. (SET / obj: "config", cfg: "getver", 매뉴얼 3.3.10)
        /// </summary>
        public Task<LifeSmartPacket> GetVersionAsync(
            LifeSmartStationConfig config,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var args = new JObject
            {
                ["cfg"] = "getver"
            };

            return SendRequestAsync(config, LifeSmartPackageType.Set, "config", args, cancellationToken);
        }

        /// <summary>
        /// Smart Station에 로컬 이벤트 수신 대상 IP와 포트를 등록합니다. (SET / obj: "config", cfg: "notify", 매뉴얼 3.3.10 & 4장)
        /// 주의: 등록 유효시간은 300초이므로, 300초 이내에 주기적으로 다시 호출해야 이벤트 수신이 유지됩니다.
        /// </summary>
        /// <param name="config">Smart Station 접속 정보</param>
        /// <param name="receiverIp">이벤트를 수신할 로컬 PC/서버의 IP 주소</param>
        /// <param name="receiverPort">이벤트를 수신할 로컬 UDP 포트 번호</param>
        public Task<LifeSmartPacket> RegisterNotifyAsync(
            LifeSmartStationConfig config,
            string receiverIp,
            int receiverPort,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var args = new JObject
            {
                ["cfg"] = "notify",
                ["host"] = receiverIp,
                ["port"] = receiverPort.ToString()
            };

            return SendRequestAsync(config, LifeSmartPackageType.Set, "config", args, cancellationToken);
        }

        /// <summary>
        /// Smart Station을 재부팅합니다. (SET / obj: "config", cfg: "reboot", 매뉴얼 3.3.10)
        /// </summary>
        public Task<LifeSmartPacket> RebootStationAsync(
            LifeSmartStationConfig config,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var args = new JObject
            {
                ["cfg"] = "reboot"
            };

            return SendRequestAsync(config, LifeSmartPackageType.Set, "config", args, cancellationToken);
        }

        /// <summary>
        /// 범용 LifeSmart Local UDP 요청을 생성하여 전송하고, 응답 패킷을 비동기로 수신합니다.
        /// 새로운 API(예: rssi, dopair, remove, doscene 등)를 추가할 때 이 메서드를 재사용할 수 있습니다.
        /// </summary>
        public async Task<LifeSmartPacket> SendRequestAsync(
            LifeSmartStationConfig config,
            LifeSmartPackageType packageType,
            string obj,
            JToken args,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            // 1) UTC 초 단위 타임스탬프와 메시지 일련번호 생성
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            int messageId = Interlocked.Increment(ref _sequenceId);

            // 2) MD5 서명 계산 (args 키 자동 오름차순 정렬)
            string signature = LifeSmartSigner.CreateSignature(
                obj,
                args,
                timestamp,
                config.Model,
                config.Token);

            // 3) 요청 JSON 구성 (매뉴얼 3.1.1 Request Data Format)
            var sysObject = new JObject
            {
                ["ver"] = JsonProtocolVersion,
                ["ts"] = timestamp,
                ["sign"] = signature,
                ["model"] = config.Model
            };

            var requestBody = new JObject
            {
                ["id"] = messageId,
                ["obj"] = obj,
                ["args"] = args ?? new JObject(),
                ["sys"] = sysObject
            };

            // 4) 10바이트 바이너리 헤더 + UTF-8 JSON 본문 패킷 인코딩
            byte[] requestPacket = LifeSmartPacket.Encode(packageType, requestBody);
            OnDebugLog?.Invoke($"[SEND -> {config.StationEndPoint}] ({packageType}, obj={obj}, id={messageId}, {requestPacket.Length} bytes)");

            await _sendLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                using (var udpClient = CreateBoundUdpClient(config.LocalPort))
                {
                    // 5) UDP 패킷 송신
                    await udpClient.SendAsync(requestPacket, requestPacket.Length, config.StationEndPoint)
                        .ConfigureAwait(false);

                    // 6) 타임아웃을 적용하여 UDP 응답 수신
                    UdpReceiveResult receiveResult = await ReceiveWithTimeoutAsync(
                        udpClient,
                        config.TimeoutMilliseconds,
                        cancellationToken).ConfigureAwait(false);

                    // 7) 응답 패킷 헤더 검증 및 JSON 파싱
                    if (!LifeSmartPacket.TryParse(receiveResult.Buffer, out LifeSmartPacket responsePacket, out string errorReason))
                    {
                        throw new InvalidOperationException($"Smart Station 응답 파싱 실패: {errorReason}");
                    }

                    OnDebugLog?.Invoke($"[RECV <- {receiveResult.RemoteEndPoint}] ({responsePacket.PackageType}, {receiveResult.Buffer.Length} bytes)");
                    return responsePacket;
                }
            }
            finally
            {
                _sendLock.Release();
            }
        }

        /// <summary>
        /// 소켓 옵션(ReuseAddress)을 먼저 설정한 뒤 로컬 포트에 바인딩된 UdpClient를 생성합니다.
        /// 주의: new UdpClient(IPEndPoint) 생성자를 쓰면 바인딩이 먼저 일어나 ReuseAddress 옵션이 무시됩니다.
        /// </summary>
        private static UdpClient CreateBoundUdpClient(int localPort)
        {
            var udpClient = new UdpClient(AddressFamily.InterNetwork);
            try
            {
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.ExclusiveAddressUse = false;
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, localPort));
                return udpClient;
            }
            catch
            {
                udpClient.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 지정된 타임아웃 내에 UDP 응답을 비동기로 대기합니다.
        /// 타임아웃 시 UI 스레드가 멈추지 않고 TimeoutException을 발생시킵니다.
        /// </summary>
        private static async Task<UdpReceiveResult> ReceiveWithTimeoutAsync(
            UdpClient udpClient,
            int timeoutMilliseconds,
            CancellationToken cancellationToken)
        {
            Task<UdpReceiveResult> receiveTask = udpClient.ReceiveAsync();
            Task delayTask = Task.Delay(timeoutMilliseconds, cancellationToken);

            Task completedTask = await Task.WhenAny(receiveTask, delayTask).ConfigureAwait(false);
            if (completedTask == receiveTask)
            {
                return await receiveTask.ConfigureAwait(false);
            }

            cancellationToken.ThrowIfCancellationRequested();
            throw new TimeoutException(
                $"Smart Station 응답 시간이 초과되었습니다 ({timeoutMilliseconds}ms). " +
                "Station IP, 네트워크 연결, OpenDev 활성화 여부 및 로컬 방화벽(UDP 포트)을 확인하세요.");
        }

        public void Dispose()
        {
            _sendLock.Dispose();
        }
    }
}
