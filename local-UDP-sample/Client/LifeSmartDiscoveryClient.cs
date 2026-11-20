using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocalUdpSample.Client
{
    /// <summary>
    /// UDP 브로드캐스트 검색으로 발견된 LifeSmart Smart Station 정보입니다.
    /// (매뉴얼 2. Discovery Protocol 참조)
    /// </summary>
    public sealed class DiscoveredStation
    {
        /// <summary>Smart Station IP 주소</summary>
        public IPAddress IpAddress { get; set; }

        /// <summary>Smart Station 고유 ID (LSID)</summary>
        public string StationId { get; set; }

        /// <summary>Smart Station 모델 타입 (MGAMOD)</summary>
        public string ModelType { get; set; }

        /// <summary>Smart Station 이름 (NAME)</summary>
        public string Name { get; set; }

        /// <summary>수신한 원문 응답 문자열</summary>
        public string RawResponse { get; set; }

        public override string ToString()
        {
            string label = !string.IsNullOrEmpty(Name) ? Name : ModelType;
            return $"{IpAddress} (LSID={StationId}, Type={label})";
        }
    }

    /// <summary>
    /// 동일 로컬 네트워크(LAN) 내에 있는 LifeSmart Smart Station을 자동으로 검색합니다.
    /// (매뉴얼 2. Discovery Protocol 참조)
    ///
    /// [동작 방식]
    /// - UDP 포트 12345로 브로드캐스트 패킷 "Z-SEARCH * \r\n" 전송
    /// - 네트워크 내 Smart Station들이 "LSID=...\nMGAMOD=...\nWLAN=...\nNAME=...\n" 형태로 응답
    /// </summary>
    public static class LifeSmartDiscoveryClient
    {
        /// <summary>Discovery 브로드캐스트 포트 (매뉴얼 2장)</summary>
        public const int DiscoveryPort = 12345;

        /// <summary>Discovery 검색 명령어 문자열</summary>
        private const string SearchCommand = "Z-SEARCH * \r\n";

        /// <summary>
        /// 로컬 네트워크에 UDP 브로드캐스트를 보내 Smart Station 목록을 검색합니다.
        /// </summary>
        /// <param name="listenDurationMs">응답 수집 대기 시간 (밀리초, 기본값: 2000ms)</param>
        /// <param name="cancellationToken">취소 토큰</param>
        /// <returns>검색된 Smart Station 목록</returns>
        public static async Task<IReadOnlyList<DiscoveredStation>> DiscoverStationsAsync(
            int listenDurationMs = 2000,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var results = new Dictionary<string, DiscoveredStation>(StringComparer.OrdinalIgnoreCase);

            using (var udpClient = new UdpClient(AddressFamily.InterNetwork))
            {
                udpClient.EnableBroadcast = true;
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 0));

                byte[] searchBytes = Encoding.UTF8.GetBytes(SearchCommand);
                var broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, DiscoveryPort);

                // UDP 브로드캐스트 전송
                await udpClient.SendAsync(searchBytes, searchBytes.Length, broadcastEndPoint)
                    .ConfigureAwait(false);

                // 지정된 시간 동안 응답 수집
                DateTime deadline = DateTime.UtcNow.AddMilliseconds(listenDurationMs);
                while (DateTime.UtcNow < deadline && !cancellationToken.IsCancellationRequested)
                {
                    int remainingMs = (int)Math.Max(1, (deadline - DateTime.UtcNow).TotalMilliseconds);
                    Task<UdpReceiveResult> receiveTask = udpClient.ReceiveAsync();
                    Task delayTask = Task.Delay(remainingMs, cancellationToken);

                    Task completed = await Task.WhenAny(receiveTask, delayTask).ConfigureAwait(false);
                    if (completed != receiveTask)
                    {
                        break;
                    }

                    UdpReceiveResult received = await receiveTask.ConfigureAwait(false);
                    string text = Encoding.UTF8.GetString(received.Buffer);

                    // 응답 내용에 LSID가 포함된 경우에만 Smart Station으로 인식합니다.
                    DiscoveredStation station = ParseResponse(received.RemoteEndPoint.Address, text);
                    if (station != null)
                    {
                        string key = !string.IsNullOrEmpty(station.StationId)
                            ? station.StationId
                            : station.IpAddress.ToString();
                        results[key] = station;
                    }
                }
            }

            return new List<DiscoveredStation>(results.Values);
        }

        /// <summary>
        /// Smart Station의 Discovery 응답 문자열("KEY=VALUE\n...")을 파싱합니다.
        /// </summary>
        private static DiscoveredStation ParseResponse(IPAddress remoteAddress, string responseText)
        {
            if (string.IsNullOrWhiteSpace(responseText))
            {
                return null;
            }

            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string[] lines = responseText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                int sep = line.IndexOf('=');
                if (sep > 0)
                {
                    string key = line.Substring(0, sep).Trim();
                    string val = line.Substring(sep + 1).Trim();
                    map[key] = val;
                }
            }

            // Smart Station 응답에는 LSID 키가 포함됩니다.
            if (!map.TryGetValue("LSID", out string lsid) || string.IsNullOrWhiteSpace(lsid))
            {
                return null;
            }

            map.TryGetValue("MGAMOD", out string modelType);
            map.TryGetValue("NAME", out string name);

            return new DiscoveredStation
            {
                IpAddress = remoteAddress,
                StationId = lsid,
                ModelType = modelType ?? string.Empty,
                Name = name ?? string.Empty,
                RawResponse = responseText.Trim()
            };
        }
    }
}
