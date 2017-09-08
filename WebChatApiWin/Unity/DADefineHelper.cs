using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebChatData.Model;
using WebChatData.MssqlDAService;
namespace WebChatApiWin
{
    public  class DADefineHelper
    {
        public static void Test()
        {
            MainDbContext<FriendData> friendDA = new MainDbContext<FriendData>("TencentWebChatDAConnString");
            try
            {
                Object e = friendDA.Entity;
            }
            catch (Exception ex) 
            {
                string msg = ex.Message;
            }
        }
    }
}
