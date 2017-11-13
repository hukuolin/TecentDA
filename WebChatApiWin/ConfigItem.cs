using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
namespace WebChatApiWin
{
    public class ConfigItem
    {
        public string WebChatLogger 
        {
            get 
            {
                return LoggerDefaultDir;
            }
        }
        /// <summary>
        /// 默认日志路径
        /// </summary>
        public static string LoggerDefaultDir 
        {
            get 
            {
                string log = ConfigurationManager.AppSettings["WebChatLogger"];
                if (string.IsNullOrEmpty(log))
                {
                    string dir = AppDomain.CurrentDomain.BaseDirectory;//debug目录
                    string cur = dir;
                    int forea = 4;
                    DirectoryInfo di = new DirectoryInfo(dir);
                    // string root = di.Root.Name;
                    while (forea > 0)
                    {
                        cur = di.Parent.FullName;
                        di = new DirectoryInfo(cur);
                        forea--;
                    }
                    log = cur;
                }
                return log + "\\" + DateTime.Now.ToString("yyyyMMddHH");
            }
        }
    }
}
