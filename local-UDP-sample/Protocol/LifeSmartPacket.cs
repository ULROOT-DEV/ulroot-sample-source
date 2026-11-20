using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LocalUdpSample.Protocol
{
    /// <summary>
    /// LifeSmart Local UDP 프로토콜 패킷의 인코딩(직렬화) 및 디코딩(역직렬화)을 담당합니다.
    /// (매뉴얼 3.1 Message Format 참조)
    ///
    /// [패킷 구조: 총 10바이트 바이너리 헤더 + UTF-8 JSON 본문]
    /// ┌─────────┬──────────┬──────────┬──────────┬─────────────────────┐
    /// │ header  │ version  │ pkg_type │ pkg_size │ content (UTF-8 JSON)│
    /// │ 2 bytes │ 2 bytes  │ 2 bytes  │ 4 bytes  │ pkg_size bytes      │
    /// │ "JL"    │ BigEndian│ BigEndian│ BigEndian│ JSON 문자열         │
    /// └─────────┴──────────┴──────────┴──────────┴─────────────────────┘
    /// </summary>
    public sealed class LifeSmartPacket
    {
        /// <summary>패킷 고정 헤더 길이 (10바이트)</summary>
        public const int HeaderLength = 10;

        /// <summary>메시지 시작 식별자 바이트 ('J' = 0x4A, 'L' = 0x4C)</summary>
        private const byte MagicByte0 = 0x4A; // 'J'
        private const byte MagicByte1 = 0x4C; // 'L'

        /// <summary>
        /// 기본 헤더 프로토콜 버전.
        /// (매뉴얼 표에는 기본값 0으로 기재되어 있으나, 실제 Smart Station 연동 시 1(0x0001)도 정상 동작합니다.)
        /// </summary>
        public const ushort DefaultHeaderVersion = 1;

        /// <summary>헤더 프로토콜 버전 (2바이트)</summary>
        public ushort Version { get; }

        /// <summary>메시지 타입 (GET, GET-REPLY, SET, SET-REPLY, NOTIFY 등)</summary>
        public LifeSmartPackageType PackageType { get; }

        /// <summary>UTF-8 JSON 원문 문자열</summary>
        public string RawJson { get; }

        /// <summary>파싱된 JSON 객체 (파싱에 실패한 경우 null)</summary>
        public JObject Payload { get; }

        public LifeSmartPacket(ushort version, LifeSmartPackageType packageType, string rawJson, JObject payload)
        {
            Version = version;
            PackageType = packageType;
            RawJson = rawJson;
            Payload = payload;
        }

        /// <summary>
        /// JSON 본문과 메시지 타입을 LifeSmart 10바이트 헤더가 포함된 UDP 전송용 바이트 배열로 변환합니다.
        /// </summary>
        /// <param name="packageType">요청 타입 (예: GET = 1, SET = 3)</param>
        /// <param name="body">전송할 JSON 객체</param>
        /// <param name="headerVersion">헤더 프로토콜 버전 (기본값: 1)</param>
        /// <returns>UDP 송신용 바이트 배열</returns>
        public static byte[] Encode(
            LifeSmartPackageType packageType,
            JObject body,
            ushort headerVersion = DefaultHeaderVersion)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            // UDP 패킷 크기를 줄이기 위해 공백/줄바꿈 없는 압축 JSON으로 직렬화합니다.
            string json = body.ToString(Formatting.None);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(json);
            uint bodyLength = (uint)bodyBytes.Length;

            byte[] packet = new byte[HeaderLength + bodyBytes.Length];

            // 1) header (2 bytes): "JL"
            packet[0] = MagicByte0;
            packet[1] = MagicByte1;

            // 2) version (2 bytes, Big-Endian / Network Byte Order)
            packet[2] = (byte)((headerVersion >> 8) & 0xFF);
            packet[3] = (byte)(headerVersion & 0xFF);

            // 3) pkg_type (2 bytes, Big-Endian)
            ushort typeValue = (ushort)packageType;
            packet[4] = (byte)((typeValue >> 8) & 0xFF);
            packet[5] = (byte)(typeValue & 0xFF);

            // 4) pkg_size (4 bytes, Big-Endian 32-bit unsigned integer)
            packet[6] = (byte)((bodyLength >> 24) & 0xFF);
            packet[7] = (byte)((bodyLength >> 16) & 0xFF);
            packet[8] = (byte)((bodyLength >> 8) & 0xFF);
            packet[9] = (byte)(bodyLength & 0xFF);

            // 5) content (pkg_size bytes, UTF-8 JSON)
            Buffer.BlockCopy(bodyBytes, 0, packet, HeaderLength, bodyBytes.Length);

            return packet;
        }

        /// <summary>
        /// Smart Station으로부터 수신한 UDP 바이트 배열을 파싱합니다.
        /// </summary>
        /// <param name="buffer">수신한 원시 바이트 배열</param>
        /// <param name="packet">파싱 성공 시 생성된 패킷 객체</param>
        /// <param name="errorReason">파싱 실패 시 사유</param>
        /// <returns>유효한 LifeSmart 패킷이면 true</returns>
        public static bool TryParse(byte[] buffer, out LifeSmartPacket packet, out string errorReason)
        {
            packet = null;
            errorReason = null;

            if (buffer == null || buffer.Length < HeaderLength)
            {
                errorReason = $"패킷 길이가 10바이트 헤더보다 짧습니다. (수신 길이: {buffer?.Length ?? 0})";
                return false;
            }

            // 시작 식별자("JL") 검증
            if (buffer[0] != MagicByte0 || buffer[1] != MagicByte1)
            {
                errorReason = $"유효하지 않은 헤더 식별자입니다. (0x{buffer[0]:X2} 0x{buffer[1]:X2}, 기대값: 'JL')";
                return false;
            }

            // Big-Endian 헤더 필드 디코딩
            ushort version = (ushort)((buffer[2] << 8) | buffer[3]);
            ushort rawType = (ushort)((buffer[4] << 8) | buffer[5]);
            uint declaredLength = ((uint)buffer[6] << 24)
                                | ((uint)buffer[7] << 16)
                                | ((uint)buffer[8] << 8)
                                | buffer[9];

            int availableLength = buffer.Length - HeaderLength;
            // 헤더에 명시된 길이(pkg_size)와 실제 수신된 본문 길이 중 안전한 범위를 사용합니다.
            int bodyLength = (declaredLength > 0 && declaredLength <= (uint)availableLength)
                ? (int)declaredLength
                : availableLength;

            string rawJson = Encoding.UTF8.GetString(buffer, HeaderLength, bodyLength);
            JObject payload = null;

            try
            {
                payload = JObject.Parse(rawJson);
            }
            catch (JsonException ex)
            {
                // JSON 본문이 깨져 있더라도 원문 확인이 가능하도록 오류 사유와 함께 반환합니다.
                errorReason = $"JSON 파싱 오류: {ex.Message}";
                packet = new LifeSmartPacket(version, (LifeSmartPackageType)rawType, rawJson, null);
                return false;
            }

            packet = new LifeSmartPacket(version, (LifeSmartPackageType)rawType, rawJson, payload);
            return true;
        }

        /// <summary>
        /// 응답 JSON을 들여쓰기된 보기 좋은 문자열로 반환합니다.
        /// </summary>
        public string ToPrettyJson()
        {
            return Payload != null
                ? Payload.ToString(Formatting.Indented)
                : RawJson;
        }
    }
}
