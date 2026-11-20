using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CloudApiSample.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudApiSample.Client
{
    /// <summary>
    /// LifeSmart Cloud Platform OpenAPI(HTTPS POST JSON) 호출 결과 모델입니다.
    /// </summary>
    public sealed class CloudApiResponse
    {
        /// <summary>HTTP 상태 코드 (예: 200)</summary>
        public int HttpStatusCode { get; }

        /// <summary>서버 응답 원문 문자열</summary>
        public string RawBody { get; }

        /// <summary>파싱된 JSON 응답 객체 (JSON이 아니면 null)</summary>
        public JObject Payload { get; }

        /// <summary>LifeSmart API 응답 코드 (0이면 성공, 매뉴얼 4.3 Error Code 참조)</summary>
        public int? Code => Payload?["code"]?.Value<int?>();

        public CloudApiResponse(int httpStatusCode, string rawBody, JObject payload)
        {
            HttpStatusCode = httpStatusCode;
            RawBody = rawBody;
            Payload = payload;
        }

        /// <summary>응답 본문을 들여쓰기된 JSON 문자열로 반환합니다.</summary>
        public string ToPrettyJson()
        {
            return Payload != null
                ? Payload.ToString(Formatting.Indented)
                : RawBody;
        }
    }

    /// <summary>
    /// LifeSmart Cloud Platform OpenAPI를 비동기로 호출하는 HTTPS 클라이언트입니다.
    ///
    /// [구현된 주요 API]
    /// - 3.4    auth.refreshtoken : UserToken 갱신
    /// - 4.5.6  EpGetAll          : 전체 디바이스 목록 조회
    /// - 4.5.7  EpGet             : 단일 디바이스 정보 조회
    /// - 4.5.8  EpSet             : 단일 디바이스 제어
    /// - 4.5.10 SceneGet          : 씬 목록 조회
    /// - 4.5.11 SceneSet          : 씬 실행(트리거)
    /// </summary>
    public sealed class LifeSmartCloudClient : IDisposable
    {
        /// <summary>OpenAPI system.ver 프로토콜 버전 (매뉴얼 4.1.2: "1.0" 고정)</summary>
        private const string ProtocolVersion = "1.0";

        private readonly HttpClient _httpClient;
        private int _sequenceId;

        /// <summary>요청/응답 요약 디버그 로그 콜백</summary>
        public Action<string> OnDebugLog { get; set; }

        public LifeSmartCloudClient(int timeoutSeconds = 15)
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };
        }

        /// <summary>
        /// 3.4 Refresh Usertoken: 만료 전 기존 usertoken을 갱신하여 새로운 usertoken을 발급받습니다.
        /// </summary>
        public async Task<CloudApiResponse> RefreshUserTokenAsync(
            CloudApiConfig config,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            config.EnsureUserCredentials();

            int id = NextMessageId();
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string sign = CloudApiSigner.CreateRefreshTokenSignature(
                config.AppKey,
                config.AppToken,
                config.UserId,
                config.UserToken,
                timestamp,
                config.Did);

            var body = new JObject
            {
                ["id"] = id,
                ["appkey"] = config.AppKey,
                ["time"] = timestamp,
                ["userid"] = config.UserId,
                ["sign"] = sign
            };

            if (!string.IsNullOrWhiteSpace(config.Did))
            {
                body["did"] = config.Did;
            }

            string url = $"{config.ServerUrl}auth.refreshtoken";
            return await PostJsonAsync(url, body, "auth.refreshtoken", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 4.5.6 EpGetAll: 계정에 연결된 전체 디바이스 목록을 조회합니다.
        /// </summary>
        /// <param name="degree">
        /// 선택 파라미터(0~3). null이면 생략(서버 기본값 2).
        /// 0: 기본 정보(agt, me, devtype, name, stat) / 1: data 포함 / 2: agt_ver, lDbm, lHeart 포함 / 3: agt_self 포함
        /// </param>
        public Task<CloudApiResponse> EpGetAllAsync(
            CloudApiConfig config,
            int? degree = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            JObject parameters = null;
            if (degree.HasValue)
            {
                parameters = new JObject
                {
                    ["degree"] = degree.Value
                };
            }

            return CallOpenApiAsync(config, "api.EpGetAll", "EpGetAll", parameters, cancellationToken);
        }

        /// <summary>
        /// 4.5.7 EpGet: 특정 Smart Station(agt)의 단일 디바이스(me) 정보를 조회합니다.
        /// </summary>
        public Task<CloudApiResponse> EpGetAsync(
            CloudApiConfig config,
            string agt,
            string me,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new JObject
            {
                ["agt"] = agt,
                ["me"] = me
            };

            return CallOpenApiAsync(config, "api.EpGet", "EpGet", parameters, cancellationToken);
        }

        /// <summary>
        /// 4.5.8 EpSet: 단일 스마트 디바이스를 제어합니다.
        /// 예: 일반 스위치/소켓 ON(type=129(0x81), val=1), OFF(type=128(0x80), val=0)
        /// </summary>
        public Task<CloudApiResponse> EpSetAsync(
            CloudApiConfig config,
            string agt,
            string me,
            string idx,
            int controlType,
            int value,
            string tag = "m",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new JObject
            {
                ["agt"] = agt,
                ["me"] = me,
                ["idx"] = idx,
                ["type"] = controlType,
                ["val"] = value
            };

            if (!string.IsNullOrWhiteSpace(tag))
            {
                parameters["tag"] = tag.Trim();
            }

            return CallOpenApiAsync(config, "api.EpSet", "EpSet", parameters, cancellationToken);
        }

        /// <summary>
        /// 4.5.10 SceneGet: 특정 Smart Station(agt)에 등록된 씬(Scene) 목록을 조회합니다.
        /// </summary>
        public Task<CloudApiResponse> SceneGetAsync(
            CloudApiConfig config,
            string agt,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new JObject
            {
                ["agt"] = agt
            };

            return CallOpenApiAsync(config, "api.SceneGet", "SceneGet", parameters, cancellationToken);
        }

        /// <summary>
        /// 4.5.11 SceneSet: 특정 Smart Station(agt)의 씬(sceneId)을 실행(트리거)합니다.
        /// (참고: 매뉴얼 4.5.11.2 서명 예시에 'method:SceneGet' 오타가 있으나, 실제 호출 메서드는 'SceneSet'입니다.)
        /// </summary>
        /// <param name="sceneType">(선택) groupsw/grouphw/grouprgbw 씬 실행 시 제어 타입 (예: 129=ON, 128=OFF)</param>
        /// <param name="rgbw">(선택) grouphw/grouprgbw 씬 실행 시 4바이트 WRGB 색상 정수값</param>
        public Task<CloudApiResponse> SceneSetAsync(
            CloudApiConfig config,
            string agt,
            string sceneId,
            int? sceneType = null,
            int? rgbw = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new JObject
            {
                ["agt"] = agt,
                ["id"] = sceneId
            };

            if (sceneType.HasValue)
            {
                parameters["type"] = sceneType.Value;
            }

            if (rgbw.HasValue)
            {
                parameters["RGBW"] = rgbw.Value;
            }

            return CallOpenApiAsync(config, "api.SceneSet", "SceneSet", parameters, cancellationToken);
        }

        /// <summary>
        /// 범용 LifeSmart Cloud OpenAPI 요청을 생성·서명하여 HTTPS POST로 전송합니다.
        /// 다른 OpenAPI 메서드(예: EpGetAllAgts, EpsSet, EpRebootAgt 등)를 확장할 때 이 메서드를 그대로 재사용할 수 있습니다.
        /// </summary>
        public async Task<CloudApiResponse> CallOpenApiAsync(
            CloudApiConfig config,
            string partialUrl,
            string method,
            JObject parameters = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            config.EnsureUserCredentials();

            if (string.IsNullOrWhiteSpace(config.ServerUrl))
            {
                throw new InvalidOperationException("OpenAPI Server URL(svrurl)을 입력하세요.");
            }

            JObject requestBody = BuildSignedRequestBody(config, method, parameters);
            string fullUrl = $"{config.ServerUrl}{partialUrl}";

            return await PostJsonAsync(fullUrl, requestBody, method, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// OpenAPI 및 WebSocket(WbAuth/RmAuth)에서 공통으로 사용하는 서명된 JSON 요청 본문을 생성합니다.
        /// </summary>
        public JObject BuildSignedRequestBody(CloudApiConfig config, string method, JObject parameters = null)
        {
            int messageId = NextMessageId();
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string sign = CloudApiSigner.CreateOpenApiSignature(method, parameters, config, timestamp);

            var systemObj = new JObject
            {
                ["ver"] = ProtocolVersion,
                ["lang"] = config.Language,
                ["userid"] = config.UserId,
                ["appkey"] = config.AppKey,
                ["time"] = timestamp,
                ["sign"] = sign
            };

            // OAuth 인증 시 did를 사용한 경우 system.did에 동일한 값을 포함해야 합니다.
            if (!string.IsNullOrWhiteSpace(config.Did))
            {
                systemObj["did"] = config.Did;
            }

            var requestBody = new JObject
            {
                ["id"] = messageId,
                ["method"] = method,
                ["system"] = systemObj
            };

            if (parameters != null && parameters.HasValues)
            {
                requestBody["params"] = parameters;
            }

            return requestBody;
        }

        /// <summary>
        /// JSON 본문을 지정된 URL로 HTTPS POST 전송하고 응답을 파싱합니다.
        /// </summary>
        private async Task<CloudApiResponse> PostJsonAsync(
            string url,
            JObject requestBody,
            string label,
            CancellationToken cancellationToken)
        {
            string json = requestBody.ToString(Formatting.None);
            OnDebugLog?.Invoke($"[HTTP POST -> {url}] ({label}, {Encoding.UTF8.GetByteCount(json)} bytes)");

            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            using (HttpResponseMessage httpResponse = await _httpClient.PostAsync(url, content, cancellationToken).ConfigureAwait(false))
            {
                string responseBody = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                OnDebugLog?.Invoke($"[HTTP {(int)httpResponse.StatusCode} <- {label}] ({Encoding.UTF8.GetByteCount(responseBody)} bytes)");

                JObject parsed = null;
                try
                {
                    if (!string.IsNullOrWhiteSpace(responseBody))
                    {
                        parsed = JObject.Parse(responseBody);
                    }
                }
                catch (JsonException)
                {
                    // 서버가 HTML/텍스트 오류 페이지를 반환한 경우 원문 그대로 확인할 수 있도록 null 유지
                }

                return new CloudApiResponse((int)httpResponse.StatusCode, responseBody, parsed);
            }
        }

        private int NextMessageId()
        {
            return Interlocked.Increment(ref _sequenceId);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
