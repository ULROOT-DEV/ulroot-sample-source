using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LocalUdpSample.Protocol
{
    /// <summary>
    /// LifeSmart Smart Station Local API 요청의 MD5 서명(sign)을 생성합니다.
    /// (매뉴얼 3.2 Signature Security Algorithm 참조)
    ///
    /// [서명 규칙 요약]
    /// 1. 원문 서명 문자열 조합 형식:
    ///    obj:&lt;obj&gt;,&lt;arg1&gt;:&lt;val1&gt;,...,&lt;argN&gt;:&lt;valN&gt;,ts:&lt;ts&gt;,model:&lt;model&gt;,token:&lt;token&gt;
    /// 2. args의 모든 키(key)는 오름차순(ASCII 사전순)으로 정렬해야 합니다.
    /// 3. args가 비어 있거나 배열(JArray) 형태인 경우(예: 다중 제어 SET eps), args 항목은 서명 계산에서 제외됩니다.
    /// 4. 조합된 문자열을 UTF-8로 인코딩한 뒤 MD5 해시(32자리 소문자 16진수)를 계산합니다.
    /// 5. Smart Station은 ts(UTC 초)와 기기 로컬 시간의 차이가 5분을 초과하면 요청을 거부합니다.
    /// </summary>
    public static class LifeSmartSigner
    {
        /// <summary>
        /// 요청 파라미터로부터 MD5 서명(sign) 값을 계산합니다.
        /// </summary>
        /// <param name="obj">요청 대상 오브젝트명 (예: "eps", "ep", "scene", "config")</param>
        /// <param name="args">요청 인자 (JObject 또는 JArray, 없으면 null)</param>
        /// <param name="timestamp">UTC Unix 타임스탬프 (초 단위)</param>
        /// <param name="model">LifeSmart에서 발급받은 OpenDev Model</param>
        /// <param name="token">LifeSmart에서 발급받은 OpenDev Token</param>
        /// <returns>32자리 소문자 MD5 해시 문자열</returns>
        public static string CreateSignature(
            string obj,
            JToken args,
            long timestamp,
            string model,
            string token)
        {
            string rawSignature = BuildRawSignatureString(obj, args, timestamp, model, token);
            return ComputeMd5LowerHex(rawSignature);
        }

        /// <summary>
        /// MD5 해시 계산 전의 "원문 서명 문자열"을 조합합니다.
        /// (디버깅 시 서명 불일치 원인을 파악할 때 이 문자열을 확인할 수 있습니다.)
        /// </summary>
        public static string BuildRawSignatureString(
            string obj,
            JToken args,
            long timestamp,
            string model,
            string token)
        {
            var segments = new List<string>
            {
                $"obj:{obj}"
            };

            // 매뉴얼 3.2.1: args가 JObject일 때만 키를 오름차순 정렬하여 서명에 포함합니다.
            // args가 JArray(배열)인 경우에는 서명 계산에서 제외합니다.
            if (args is JObject argsObj && argsObj.HasValues)
            {
                IEnumerable<JProperty> sortedProperties = argsObj
                    .Properties()
                    .OrderBy(p => p.Name, StringComparer.Ordinal);

                foreach (JProperty property in sortedProperties)
                {
                    // 개별 프로퍼티가 배열(JArray)이면 매뉴얼 규칙에 따라 서명에서 제외합니다.
                    if (property.Value.Type == JTokenType.Array)
                    {
                        continue;
                    }

                    string valueText = FormatArgValue(property.Value);
                    segments.Add($"{property.Name}:{valueText}");
                }
            }

            segments.Add($"ts:{timestamp}");
            segments.Add($"model:{model}");
            segments.Add($"token:{token}");

            return string.Join(",", segments);
        }

        /// <summary>
        /// JToken 값을 서명 문자열에 들어갈 텍스트로 변환합니다.
        /// </summary>
        private static string FormatArgValue(JToken value)
        {
            if (value == null || value.Type == JTokenType.Null)
            {
                return string.Empty;
            }

            switch (value.Type)
            {
                case JTokenType.Boolean:
                    return value.Value<bool>() ? "true" : "false";
                case JTokenType.String:
                    return value.Value<string>();
                case JTokenType.Object:
                    return value.ToString(Formatting.None);
                default:
                    return value.ToString();
            }
        }

        /// <summary>
        /// 입력 문자열의 UTF-8 바이트에 대한 MD5 해시를 32자리 소문자 16진수로 반환합니다.
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
