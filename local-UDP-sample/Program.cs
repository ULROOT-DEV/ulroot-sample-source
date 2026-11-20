using System;
using System.Windows.Forms;

namespace LocalUdpSample
{
    internal static class Program
    {
        /// <summary>
        /// LifeSmart Local UDP 샘플 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
