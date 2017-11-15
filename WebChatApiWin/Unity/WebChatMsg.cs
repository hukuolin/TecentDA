using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebChatApiWin
{
    public class WebChatMsg
    {
        public int AddMsgCount { get; set; }
        public int ContinueFlag { get;set;}
        public int DelContactCount { get; set; }
        public int ModChatRoomMemberCount { get; set; }
        public int ModContactCount { get; set; }
        public string SKey { get; set; }
        public TecentBaseResponse BaseResponse { get; set; }
        public MsgProfileItem Profile { get; set; }
    }
    public class Msg
    {
        public int AppMsgType { get; set; }
        public string Content { get; set; }
        public string CreateTime { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string ForwardFlag { get; set; }
        public string FromUserName { get; set; }//消息来源【好友或者消息群】
        public string HasProductId { get; set; }
        public int ImgHeight { get; set; }
        public int ImgStatus { get; set; }
        public int ImgWidth { get; set; }
        public string MediaId { get; set; }
        public string MsgId { get; set; }
        public int MsgType { get; set; }
        public string NewMsgId { get; set; }
        public string OriContent { get; set; }
        public int PlayLength { get; set; }
        public int Status { get; set; }
        public int StatusNotifyCode { get; set; }
        public string StatusNotifyUserName { get; set; }
        public int SubMsgType { get; set; }
        public string Ticket { get; set; }
        public string ToUserName { get; set; }//消息接收人
        public string Url { get; set; }
        public int VoiceLength { get; set; }
        public List<MsgRecommendInfo> RecommendInfo { get; set; }
        public AppInfoItem AppInfo { get; set; }
    }
    public class AppInfoItem
    {
        public string AppID { get; set; }
        public int Type { get; set; }
    }
    public class MsgRecommendInfo
    {
        public string Alias { get; set; }
        public int AttrStatus { get; set; }
        public string City { get; set; }
        public string Content { get; set; }
        public string NickName { get; set; }
        public int OpCode { get; set; }
        public string Province { get; set; }
        public int  QQNum { get; set; }
        public int Scene { get; set; }
        public int Sex { get; set; }
        public string Signature { get; set; }
        public string Ticket { get; set; }
        public string UserName { get; set; }
        public int VerifyFlag { get; set; } 
    }
    public class MsgProfileItem
    {
        public string Alias { get; set; }
        public int BindUin { get; set; }
        public int BitFlag { get; set; }
        public int HeadImgUpdateFlag { get; set; }
        public string HeadImgUrl { get; set; }
        public int PersonalCard { get; set; }
        public int Sex { get; set; }
        public string Signature { get; set; }
        public string Status { get; set; }
        public TecentUser UserName { get; set; }
        public TecentUser BindEmail { get; set; }
        public TecentUser BindMobile { get; set; }
        public TecentUser NickName { get; set; }
    }
    public class TecentUser
    {
        public string Buff { get; set; }
    }
}
