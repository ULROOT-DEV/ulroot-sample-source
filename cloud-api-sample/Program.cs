using System;
using System.Net;
using System.Windows.Forms;

namespace CloudApiSample
{
    internal static class Program
    {
        /// <summary>
        /// LifeSmart Cloud API 샘플 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // .NET Framework 4.8 환경에서 HTTPS / WSS 통신 시 TLS 1.2를 명시적으로 활성화합니다.
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
