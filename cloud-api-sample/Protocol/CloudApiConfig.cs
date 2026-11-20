using System;

namespace CloudApiSample.Protocol
{
    /// <summary>
    /// LifeSmart Cloud Platform OpenAPI 및 WebSocket 호출에 필요한 인증/서버 설정 모델입니다.
    /// </summary>
    public sealed class CloudApiConfig
    {
        /// <summary>LifeSmart Open Platform에서 발급받은 Application Key</summary>
        public string AppKey { get; }

        /// <summary>LifeSmart Open Platform에서 발급받은 Application Token (서명 전용, 외부 유출 주의)</summary>
        public string AppToken { get; }

        /// <summary>사용자 OAuth 인증 후 발급받은 User ID (user_no)</summary>
        public string UserId { get; }

        /// <summary>사용자 OAuth 인증 후 발급받은 User Token</summary>
        public string UserToken { get; }

        /// <summary>
        /// (선택) 멀티 단말 구분용 Device Unique ID.
        /// 주의: OAuth 인증(3.1) 시 did를 포함해 토큰을 발급받았다면,
        /// 이후 모든 OpenAPI / WebSocket 호출의 system.did에도 반드시 동일한 값을 전달해야 합니다.
        /// </summary>
        public string Did { get; }

        /// <summary>OpenAPI 기본 URL (예: https://api.us.ilifesmart.com/app/)</summary>
        public string ServerUrl { get; }

        /// <summary>WebSocket 접속 URL (예: wss://api.us.ilifesmart.com:8443/wsapp/)</summary>
        public string WebSocketUrl { get; }

        /// <summary>요청 언어 코드 (기본값: "en")</summary>
        public string Language { get; }

        public CloudApiConfig(
            string appKey,
            string appToken,
            string userId,
            string userToken,
            string serverUrl,
            string webSocketUrl,
            string did = null,
            string language = "en")
        {
            if (string.IsNullOrWhiteSpace(appKey))
            {
                throw new ArgumentException("AppKey를 입력하세요.", nameof(appKey));
            }

            if (string.IsNullOrWhiteSpace(appToken))
            {
                throw new ArgumentException("AppToken을 입력하세요.", nameof(appToken));
            }

            AppKey = appKey.Trim();
            AppToken = appToken.Trim();
            UserId = (userId ?? string.Empty).Trim();
            UserToken = (userToken ?? string.Empty).Trim();
            Did = (did ?? string.Empty).Trim();
            ServerUrl = CloudRegionInfo.NormalizeBaseUrl(serverUrl);
            WebSocketUrl = (webSocketUrl ?? string.Empty).Trim();
            Language = string.IsNullOrWhiteSpace(language) ? "en" : language.Trim();
        }

        /// <summary>
        /// UserId와 UserToken이 모두 입력되어 있는지 검증합니다.
        /// </summary>
        public void EnsureUserCredentials()
        {
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new InvalidOperationException("User ID (user_no)를 입력하세요.");
            }

            if (string.IsNullOrWhiteSpace(UserToken))
            {
                throw new InvalidOperationException("UserToken을 입력하세요.");
            }
        }
    }
}
