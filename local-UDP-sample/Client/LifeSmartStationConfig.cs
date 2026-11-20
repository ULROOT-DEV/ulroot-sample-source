using System;
using System.Net;

namespace LocalUdpSample.Client
{
    /// <summary>
    /// LifeSmart Smart Station과의 Local UDP 통신에 필요한 연결 설정 정보입니다.
    /// </summary>
    public sealed class LifeSmartStationConfig
    {
        /// <summary>Smart Station이 로컬 API 요청을 수신하는 기본 UDP 포트 (매뉴얼 3장)</summary>
        public const int DefaultStationPort = 12348;

        /// <summary>
        /// 클라이언트(서드파티 앱) 기본 응답 수신 포트 (매뉴얼 1.1 / 3장).
        /// - 앱 템플릿(App Enabling) 방식: Smart Station이 무조건 클라이언트 12346 포트로 응답합니다.
        /// - QR 코드 스캔(Scanning Code) 방식: 클라이언트가 요청을 보낸 송신 포트로 응답합니다.
        /// 따라서 로컬 포트를 12346으로 바인딩하면 두 활성화 방식 모두에서 정상적으로 응답을 받을 수 있습니다.
        /// </summary>
        public const int DefaultLocalPort = 12346;

        /// <summary>Smart Station IP 주소</summary>
        public IPAddress StationAddress { get; }

        /// <summary>Smart Station UDP 요청 포트 (기본값: 12348)</summary>
        public int StationPort { get; }

        /// <summary>로컬 송수신 바인딩 포트 (기본값: 12346, 0 지정 시 OS가 임의 포트 할당)</summary>
        public int LocalPort { get; }

        /// <summary>LifeSmart에서 발급받은 OpenDev Model</summary>
        public string Model { get; }

        /// <summary>LifeSmart에서 발급받은 OpenDev Token</summary>
        public string Token { get; }

        /// <summary>UDP 응답 대기 타임아웃 (밀리초, 기본값: 5000ms)</summary>
        public int TimeoutMilliseconds { get; }

        public LifeSmartStationConfig(
            IPAddress stationAddress,
            string model,
            string token,
            int stationPort = DefaultStationPort,
            int localPort = DefaultLocalPort,
            int timeoutMilliseconds = 5000)
        {
            StationAddress = stationAddress ?? throw new ArgumentNullException(nameof(stationAddress));

            if (string.IsNullOrWhiteSpace(model))
            {
                throw new ArgumentException("Model 값이 비어 있습니다.", nameof(model));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token 값이 비어 있습니다.", nameof(token));
            }

            Model = model.Trim();
            Token = token.Trim();
            StationPort = stationPort;
            LocalPort = localPort;
            TimeoutMilliseconds = timeoutMilliseconds;
        }

        /// <summary>Smart Station의 UDP 엔드포인트를 반환합니다.</summary>
        public IPEndPoint StationEndPoint => new IPEndPoint(StationAddress, StationPort);
    }
}
