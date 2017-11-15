using Domain.CommonData;
using Infrastructure.ExtService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace WebChatApiWin
{
    public static class AppLoggerService
    {
        /// <summary>
        ///使用日志功能【配置文件中OpenLogFun<> true则不写日志】
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logType"></param>
        public static void CreateLog(this string msg,ELogType logType) 
        {
            string cfg = ConfigurationManager.AppSettings["OpenLogFun"];
            if (cfg!="true")
            {
                return;
            }
            LoggerWriter.CreateLogFile(msg, NowAppDirHelper.GetNowAppDir(AppCategory.WinApp) + "/" + logType, logType);
        }
    }
}
