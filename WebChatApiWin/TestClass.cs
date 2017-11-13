using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.CommonData;
using System.Configuration;
using System.Net;

namespace WebChatApiWin
{
    public class TestClass
    {
        public void ConvertInt64(string uin) 
        {
            string[] sp = uin.Split('0');
            List<string> bts = new List<string>();
            foreach (var item in sp)
            {
             //  int t= int.Parse(item, System.Globalization.NumberStyles.HexNumber);
            }
           
        }
        /// <summary>
        /// 获取js所表示Date.now()的毫秒级数据
        /// </summary>
        /// <returns></returns>
        public long GetJsDateNow() 
        {
            DateTime now = DateTime.Now;
            //Date.now js
            long tick = now.Ticks;
            DateTime utc = new DateTime(1970, 1, 1);
            long mils = (tick - utc.Ticks) / 10000;
            return mils;
        }
        /// <summary>
        /// 获取js表示的 ~new Date  时间戳
        /// </summary>
        /// <returns></returns>
        public string GetJsNewData() 
        {
            long mils = GetJsDateNow();
            return ((long)mils / 1138.8).ToString();
        }
        /*
         1.获取登录的skey
         * POST https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxinit?r=$r$ 
         *  "/cgi-bin/mmwebwx-bin/webwxinit?r=" + ~new Date  这里 ~ new Date是获取什么时间【~ 位 非运算】 获取数据的反码 实际上就是毫秒级数据的反码
         * 
         * 需要提取数据  
         * Uin：   this.getUserInfo() && this.getUserInfo().Uin || a.getCookie("wxuin")
         * Sid：a.getCookie("wxsid")
         * DeviceID："e" + ("" + Math.random().toFixed(15)).substring(2, 17)   如："e485887172927031" 生成15位的随机浮点数 并获取对应字符串形式
         * 
         * 2.获取指定群的成员
         * Get https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxgetcontact?r=1498406493586&seq=0&skey=@crypt_45ae67ad_25514fafe82ebe778cf4986cd6d08f4f
         */
        public void GetWebGroupMembers(string loginCookie) 
        {
            ConfigItem web = new ConfigItem();
            string dir= web.WebChatLogger;
            LoggerWriter.CreateLogFile(loginCookie, dir, ELogType.SessionOrCookieLog);
        }
        public void GetWebGroupMembers(CookieCollection cookie)
        {
            foreach (var item in cookie)
            {
               
            }
        }
        public string  Convert2String(long milSec)
        {
            int max = int.MaxValue;//int 类型最大值
            string s = Convert.ToString(milSec, 2);
            return s;
        }
        /// <summary>
        /// 生成js等效的~ 64 位机器上提取低32 位
        /// </summary>
        /// <param name="milSec"></param>
        public string  GetNegateString(long milSec) 
        {
            string s = Convert.ToString(milSec, 2);//不足64位补足
            string pcString = s.PadLeft(64, '0');
            pcString = pcString.Replace("1", "2");
            pcString = pcString.Replace("0", "1");
            pcString = pcString.Replace("2", "0");
            //然后提取低32 位
            s = pcString.Substring(32);
            long l= Convert.ToInt64(s,2);
            return l.ToString();
        }
        /// <summary>
        /// 生成js日期戳的取反串
        /// </summary>
        public string GenerateJsNegate() 
        {
            DateTime now = DateTime.Now;
            //Date.now js
            long tick = now.Ticks;
            DateTime utc = new DateTime(1970, 1, 1);
            long milSec = (tick - utc.Ticks) / 10000;

            string s = Convert.ToString(milSec, 2);//不足64位补足
            string pcString = s.PadLeft(64, '0');
            pcString = pcString.Replace("1", "2");
            pcString = pcString.Replace("0", "1");
            pcString = pcString.Replace("2", "0");
            //然后提取低32 位
            s = pcString.Substring(32);
            long l = Convert.ToInt64(s, 2);
            return l.ToString();
        }
        public void WriteLoginCookie(string text) 
        {
            ConfigItem web = new ConfigItem();
            string dir = web.WebChatLogger;
            LoggerWriter.CreateLogFile(text, dir, ELogType.DataLog);
        }
    }
}
