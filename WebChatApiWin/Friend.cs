using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WebChatData.Model;
namespace WebChatApiWin
{
    /// <summary>
    /// 查询获取的好友列表数据
    /// </summary>
    public class FriendDataResponse 
    {
        public BaseResponse BaseResponse { get; set; }
        public int MemberCount { get; set; }
        public List<Friend> MemberList { get; set; }
    }
    public class BaseResponse 
    {
        public int Ret { get; set; }
        public string ErrMsg { get; set; }
    }
  
    /// <summary>
    /// 发送微信消息返回的内容
    /// </summary>
    public class SendMsgResponse 
    {
        public string MsgID { get; set; }
        public string LocalID { get; set; }
        public SendMsgResponse BaseResponse { get; set; }
    }
  
}
