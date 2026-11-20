using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudApiSample.Protocol
{
    /// <summary>
    /// LifeSmart Cloud Platform의 서비스 리전(svrrgnid)별 OpenAPI URL 및 WebSocket URL 매핑 정보입니다.
    /// (매뉴얼 7.2 URL 및 Appendix 1 / Appendix 2 참조)
    ///
    /// - 사용자 OAuth 인증(3.3장) 완료 후 응답으로 받은 svrurl / svrrgnid에 맞는 서버를 사용해야 합니다.
    /// - 한국(kr) 계정은 매뉴얼 Appendix 1 기준 AME 리전에 매핑되어 있습니다.
    /// </summary>
    public sealed class CloudRegionInfo
    {
        /// <summary>서버 리전 ID (svrrgnid, 예: AME, GS, APZ, EUROPE, JAP 등)</summary>
        public string RegionId { get; }

        /// <summary>화면 표시용 설명</summary>
        public string DisplayName { get; }

        /// <summary>HTTPS OpenAPI 기본 주소 (svrurl, 항상 '/'로 끝남)</summary>
        public string OpenApiBaseUrl { get; }

        /// <summary>실시간 이벤트 수신용 WebSocket 주소 (WSS)</summary>
        public string WebSocketUrl { get; }

        public CloudRegionInfo(string regionId, string displayName, string openApiBaseUrl, string webSocketUrl)
        {
            RegionId = regionId;
            DisplayName = displayName;
            OpenApiBaseUrl = openApiBaseUrl;
            WebSocketUrl = webSocketUrl;
        }

        public override string ToString()
        {
            return $"{RegionId} - {DisplayName}";
        }

        /// <summary>
        /// 매뉴얼 Appendix 2 및 7.2장에 정의된 전체 리전 목록입니다.
        /// </summary>
        public static IReadOnlyList<CloudRegionInfo> AllRegions { get; } = new List<CloudRegionInfo>
        {
            new CloudRegionInfo("AME",    "America / Korea (us)",  "https://api.us.ilifesmart.com/app/",  "wss://api.us.ilifesmart.com:8443/wsapp/"),
            new CloudRegionInfo("GS",     "Global / Default",      "https://api.ilifesmart.com/app/",     "wss://api.ilifesmart.com:8443/wsapp/"),
            new CloudRegionInfo("APZ",    "Asia Pacific (apz)",    "https://api.apz.ilifesmart.com/app/", "wss://api.apz.ilifesmart.com:8443/wsapp/"),
            new CloudRegionInfo("EUROPE", "Europe (eur)",          "https://api.eur.ilifesmart.com/app/", "wss://api.eur.ilifesmart.com/wsapp/"),
            new CloudRegionInfo("JAP",    "Japan (jp)",            "https://api.jp.ilifesmart.com/app/",  "wss://api.jp.ilifesmart.com:8443/wsapp/"),
            new CloudRegionInfo("VIP1",   "China VIP1 (cn1)",      "https://api.cn1.ilifesmart.com/app/", "wss://api.cn1.ilifesmart.com:8443/wsapp/"),
            new CloudRegionInfo("CN2",    "China CN2 (cn2)",       "https://api.cn2.ilifesmart.com/app/", "wss://api.cn2.ilifesmart.com:8443/wsapp/"),
            new CloudRegionInfo("CN0",    "China CN0 (cn0)",       "https://api.cn0.ilifesmart.com/app/", "wss://api.ilifesmart.com:8443/wsapp/")
        };

        /// <summary>
        /// svrrgnid 문자열로 해당하는 리전 정보를 찾습니다. (일치 항목이 없으면 null 반환)
        /// </summary>
        public static CloudRegionInfo FindByRegionId(string regionId)
        {
            if (string.IsNullOrWhiteSpace(regionId))
            {
                return null;
            }

            string normalized = regionId.Trim();
            // Appendix 1의 약어(EUR 등)가 전달될 경우도 안전하게 매핑합니다.
            if (string.Equals(normalized, "EUR", StringComparison.OrdinalIgnoreCase))
            {
                normalized = "EUROPE";
            }

            return AllRegions.FirstOrDefault(r =>
                string.Equals(r.RegionId, normalized, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// OAuth 응답의 svrurl 문자열로부터 해당하는 리전 정보를 추정합니다.
        /// </summary>
        public static CloudRegionInfo FindByOpenApiUrl(string svrUrl)
        {
            if (string.IsNullOrWhiteSpace(svrUrl))
            {
                return null;
            }

            string normalized = NormalizeBaseUrl(svrUrl);
            return AllRegions.FirstOrDefault(r =>
                string.Equals(r.OpenApiBaseUrl, normalized, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// OpenAPI 기본 URL이 '/'로 끝나도록 정규화합니다.
        /// </summary>
        public static string NormalizeBaseUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            string trimmed = url.Trim();
            return trimmed.EndsWith("/", StringComparison.Ordinal) ? trimmed : trimmed + "/";
        }
    }
}
