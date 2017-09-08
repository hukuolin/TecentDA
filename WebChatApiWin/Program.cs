using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WebChatApiWin
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            TestClass tc = new TestClass();
            //tc.GetNegateString(1498573135693);
            //tc.GetWebGroupMembers(string.Empty);
            //tc.ConvertInt64("dc01fb086e6309a7e3765589c84a3f9452a5f97725780e4da7d7b0cbb090d29c");
            Application.Run(new MainForm());
        }
    }
}
