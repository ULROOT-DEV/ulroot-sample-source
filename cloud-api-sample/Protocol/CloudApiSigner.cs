using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudApiSample.Protocol
{
    /// <summary>
    /// LifeSmart Cloud Platform API에서 사용하는 3가지 MD5 서명(sign) 생성 규칙을 구현합니다.
    ///
    /// 1. 사용자 인증 팝업 URL 서명 (매뉴얼 3.2장)
    ///    appkey=***&amp;auth_callback=***[&amp;did=***]&amp;time=***&amp;apptoken=***
    ///    (lang은 서명에서 제외, auth_callback은 URL 인코딩 전 원문 사용)
    ///
    /// 2. UserToken 갱신 서명 (매뉴얼 3.4장)
    ///    appkey=***[&amp;did=***]&amp;time=***&amp;userid=***&amp;apptoken=***&amp;usertoken=***
    ///
    /// 3. OpenAPI 및 WebSocket(WbAuth/RmAuth) 서명 (매뉴얼 4.2.1 / 7.3 / 7.4장)
    ///    method:***,[params 오름차순 key:val,...],[did:***,]time:***,userid:***,usertoken:***,appkey:***,apptoken:***
    ///
    /// 모든 서명은 UTF-8 인코딩 후 MD5 해시(32자리 소문자 16진수)로 계산하며,
    /// time(UTC 초)과 클라우드 서버 시각의 차이가 5분을 초과하면 요청이 거부됩니다.
    /// </summary>
    public static class CloudApiSigner
    {
        #region 3.2 사용자 인증(auth.authorize) URL 및 서명

        /// <summary>
        /// 매뉴얼 3.2 규칙에 따라 auth.authorize 원문 서명 문자열을 조합합니다.
        /// </summary>
        public static string BuildAuthorizeRawSignature(
            string appKey,
            string appToken,
            string authCallback,
            long timestamp,
            string did = null)
        {
            // apptoken을 제외한 파라미터를 키 기준 알파벳 오름차순 정렬합니다.
            var pairs = new SortedDictionary<string, string>(StringComparer.Ordinal)
            {
                ["appkey"] = appKey,
                ["auth_callback"] = authCallback,
                ["time"] = timestamp.ToString()
            };

            if (!string.IsNullOrWhiteSpace(did))
            {
                pairs["did"] = did.Trim();
            }

            var sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in pairs)
            {
                if (sb.Length > 0)
                {
                    sb.Append('&');
                }
                sb.Append(kv.Key).Append('=').Append(kv.Value);
            }

            // apptoken은 항상 맨 마지막에 붙입니다.
            sb.Append("&apptoken=").Append(appToken);
            return sb.ToString();
        }

        /// <summary>
        /// 매뉴얼 3.2 규칙에 따라 auth.authorize MD5 서명을 계산합니다.
        /// </summary>
        public static string CreateAuthorizeSignature(
            string appKey,
            string appToken,
            string authCallback,
            long timestamp,
            string did = null)
        {
            string raw = BuildAuthorizeRawSignature(appKey, appToken, authCallback, timestamp, did);
            return ComputeMd5LowerHex(raw);
        }

        /// <summary>
        /// 매뉴얼 3.1 형식의 사용자 OAuth 인증 요청 URL(auth.authorize)을 생성합니다.
        /// </summary>
        public static string BuildAuthorizeRequestUrl(
            string serverBaseUrl,
            int messageId,
            string appKey,
            string appToken,
            string authCallback,
            long timestamp,
            string did = null,
            string lang = "en")
        {
            string normalizedBase = CloudRegionInfo.NormalizeBaseUrl(serverBaseUrl);
            string sign = CreateAuthorizeSignature(appKey, appToken, authCallback, timestamp, did);

            var queryParts = new List<string>
            {
                $"id={messageId}",
                $"appkey={Uri.EscapeDataString(appKey)}",
                $"time={timestamp}",
                $"auth_callback={Uri.EscapeDataString(authCallback)}"
            };

            if (!string.IsNullOrWhiteSpace(did))
            {
                queryParts.Add($"did={Uri.EscapeDataString(did.Trim())}");
            }

            queryParts.Add($"sign={sign}");

            if (!string.IsNullOrWhiteSpace(lang))
            {
                // lang은 URL 쿼리에는 포함되지만 서명 계산에서는 제외됩니다.
                queryParts.Add($"lang={Uri.EscapeDataString(lang.Trim())}");
            }

            return $"{normalizedBase}auth.authorize?{string.Join("&", queryParts)}";
        }

        #endregion

        #region 3.4 UserToken 갱신(auth.refreshtoken) 서명

        /// <summary>
        /// 매뉴얼 3.4 규칙에 따라 auth.refreshtoken 원문 서명 문자열을 조합합니다.
        /// </summary>
        public static string BuildRefreshTokenRawSignature(
            string appKey,
            string appToken,
            string userId,
            string userToken,
            long timestamp,
            string did = null)
        {
            var pairs = new SortedDictionary<string, string>(StringComparer.Ordinal)
            {
                ["appkey"] = appKey,
                ["time"] = timestamp.ToString(),
                ["userid"] = userId
            };

            if (!string.IsNullOrWhiteSpace(did))
            {
                pairs["did"] = did.Trim();
            }

            var sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in pairs)
            {
                if (sb.Length > 0)
                {
                    sb.Append('&');
                }
                sb.Append(kv.Key).Append('=').Append(kv.Value);
            }

            // apptoken과 usertoken은 맨 마지막에 순서대로 붙입니다.
            sb.Append("&apptoken=").Append(appToken);
            sb.Append("&usertoken=").Append(userToken);
            return sb.ToString();
        }

        /// <summary>
        /// 매뉴얼 3.4 규칙에 따라 auth.refreshtoken MD5 서명을 계산합니다.
        /// </summary>
        public static string CreateRefreshTokenSignature(
            string appKey,
            string appToken,
            string userId,
            string userToken,
            long timestamp,
            string did = null)
        {
            string raw = BuildRefreshTokenRawSignature(appKey, appToken, userId, userToken, timestamp, did);
            return ComputeMd5LowerHex(raw);
        }

        #endregion

        #region 4.2 OpenAPI 및 7.3/7.4 WebSocket 서명

        /// <summary>
        /// 매뉴얼 4.2.1 규칙에 따라 OpenAPI / WebSocket 원문 서명 문자열을 조합합니다.
        /// 형식: method:M[,k1:v1,k2:v2,...][,did:D],time:T,userid:U,usertoken:UT,appkey:AK,apptoken:AT
        /// </summary>
        public static string BuildOpenApiRawSignature(
            string method,
            JObject parameters,
            CloudApiConfig config,
            long timestamp)
        {
            var segments = new List<string>
            {
                $"method:{method}"
            };

            // params가 존재하면 모든 키를 ASCII 오름차순으로 정렬하여 key:value 형태로 추가합니다.
            if (parameters != null && parameters.HasValues)
            {
                IEnumerable<JProperty> sortedProps = parameters
                    .Properties()
                    .OrderBy(p => p.Name, StringComparer.Ordinal);

                foreach (JProperty prop in sortedProps)
                {
                    if (prop.Value == null || prop.Value.Type == JTokenType.Null)
                    {
                        continue;
                    }

                    segments.Add($"{prop.Name}:{FormatParamValue(prop.Value)}");
                }
            }

            // did가 지정된 경우에만 서명에 포함합니다.
            if (!string.IsNullOrWhiteSpace(config.Did))
            {
                segments.Add($"did:{config.Did}");
            }

            segments.Add($"time:{timestamp}");
            segments.Add($"userid:{config.UserId}");
            segments.Add($"usertoken:{config.UserToken}");
            segments.Add($"appkey:{config.AppKey}");
            segments.Add($"apptoken:{config.AppToken}");

            return string.Join(",", segments);
        }

        /// <summary>
        /// 매뉴얼 4.2.1 규칙에 따라 OpenAPI / WebSocket MD5 서명을 계산합니다.
        /// </summary>
        public static string CreateOpenApiSignature(
            string method,
            JObject parameters,
            CloudApiConfig config,
            long timestamp)
        {
            string raw = BuildOpenApiRawSignature(method, parameters, config, timestamp);
            return ComputeMd5LowerHex(raw);
        }

        /// <summary>
        /// JToken 값을 서명 문자열용 텍스트로 변환합니다.
        /// </summary>
        private static string FormatParamValue(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Boolean:
                    return token.Value<bool>() ? "true" : "false";
                case JTokenType.String:
                    return token.Value<string>();
                case JTokenType.Object:
                case JTokenType.Array:
                    return token.ToString(Formatting.None);
                default:
                    return token.ToString();
            }
        }

        #endregion

        /// <summary>
        /// 입력 문자열의 UTF-8 바이트에 대한 MD5 해시를 32자리 소문자 16진수 문자열로 반환합니다.
        /// </summary>
        public static string ComputeMd5LowerHex(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hashBytes.Length * 2);
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
