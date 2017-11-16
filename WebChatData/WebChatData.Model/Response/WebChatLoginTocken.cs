using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using Infrastructure.ExtService;
namespace WebChatData.Model
{
    public class WebChatLoginTocken
    {
        public int ret { get; set; }
        public string message { get; set; }
        public string skey { get; set; }
        public string wxsid { get; set; }
        public string wxuin { get; set; }
        public string pass_ticket { get; set; }
        public string isgrayscale { get; set; }
    }
    public class QueryWebChatMsgObjectParam
    {
        public QueryWebChatBaseRequestParam BaseRequest { get; set; }
        public string rr { get; set; }//时间戳  ~new Date
        public TecentSyncKey SyncKey { get; set; }
       // public QueryWebChatMsgSyncKey SyncKey { get; set; }
    }
    public class QueryWebChatBaseRequestParam
    {
        public string DeviceID { get; set; }
        public string Sid { get; set; }
        public string Skey { get; set; }
        public string Uin { get; set; }

    }
    //public class QueryWebChatMsgSyncKey
    //{
    //    public int Count { get; set; }
    //    public QueryWebChatMsgDict List { get; set; }
    //}
    /// <summary>
    /// 查询的消息列表
    /// </summary>
    public class QueryWebChatMsgDict
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
    public static class HttpResponseHelp
    {
        public static void FromHttpResponseGetEntity<T>(this T obj, string text) where T : class
        {
             obj.FromXmlStringGetEntity(text);
        }
        
    }
    public static class WebChatHelper
    {
        public static void GetWebChatLoginTocket(this WebChatLoginTocken tocket,string text) 
        {
            tocket.FromXmlStringGetEntity(text);
        }
        /// <summary>
        /// 根据用户登录的tocket信息生成当前查询微信消息的URL
        /// </summary>
        /// <param name="tocket"></param>
        /// <param name="msgQueryUrlFormat"></param>
        /// <returns></returns>
        public static string GenerateQueryWebChatMsgUrl(this WebChatLoginTocken tocket, string msgQueryUrlFormat)
        {
            return tocket.FillStringFromObject(msgQueryUrlFormat);
        }
    }

    public class QueryWebChatMsgSyncKeyResponse
    {
        public TecentBaseResponse BaseResponse { get; set; }
        public string ChatSet { get; set; }
        public string ClickReportInterval { get; set; }
        public string ClientVersion { get; set; }
        public int Count { get; set; }
        public int GrayScale { get; set; }
        public int InviteStartCount { get; set; }
        public int MPSubscribeMsgCount { get; set; }
        public string SKey { get; set; }
        public string SystemTime { get; set; }
    }
    /// <summary>
    /// 接口查询消息列表参数WebChatMsgSyncKey
    /// </summary>
    public class TecentWebChatMsgSyncKey
    {
        public TecentSyncKey SyncKey { get; set; }
        public TecentBaseResponse BaseResponse { get; set; }
        public List<TecentContactList> ContactList { get; set; }
        public List<WebChatMPSubscribeMsgList> MPSubscribeMsgList { get; set; }
    }
    public class TecentBaseResponse
    {
        public string ErrMsg { get; set; }
        public int Ret { get; set; }

    }
    public class TecentContactList : TecentUserCommonFields
    {
        public string Alias { get; set; }
        public string AppAccountFlag { get; set; }
        public string ChatRoomId { get; set; }
        public string City { get; set; }
        public string ContactFlag { get;set; }
        public string EncryChatRoomId { get; set; }
        public string HeadImgUrl { get; set; }
        public string HideInputBarFlag { get;set;}
        public string IsOwner { get; set; }
        public string MemberCount { get; set; }
        public string OwnerUin { get; set; }
        public string Province { get; set; }
        public string RemarkName { get; set; }
        public int Sex { get; set; }
        public string Signature { get; set; }
        public string SnsFlag { get; set; }
        public string StarFriend { get; set; }
        public int Statues { get; set; }
        public int UniFriend { get; set; }
        public string VerifyFlag { get; set; }
        public List<WebChatGroupMember> MemberList { get; set; }
       
       
    }
    public class WebChatGroupMember : TecentUserCommonFields
    {
        public string MemberStatus { get; set; }
       
    }
    public class TecentUserCommonFields : TecentAccountBaseField
    {
        public string AttrStatus { get; set; }
        public string DisplayName { get; set; }
        public string KeyWord { get; set; }
        public string PYInitial { get; set; }
        public string PYQuanPin { get; set; }
        public string RemarkPYInitial { get; set; }
        public string RemarkPYQuanPin { get; set; }
        public string Uin { get; set; }
    }
    public class TecentSyncKey
    {
        public int Count { get; set; }
        public List<SyncKeyItem> List { get; set; }
    }
    public class SyncKeyItem
    {//检索消息时使用的键值
        public string Key { get; set; }
        public string Val { get; set; }
    }
    public class TecentAccountBaseField 
    {//腾讯账户公用字段
        public string NickName { get; set; }
        public string UserName { get; set; }
    }
    public class WebChatMPSubscribeMsgList : TecentAccountBaseField
    {//公众号 
        public string MPArticleCount { get; set; }
        public string Time { get; set; }
        public List<MPArticleListMsg> MPArticleList { get; set; }
    }
    public class MPArticleListMsg
    { //公众号消息
        public string Cover { get; set; }
        public string Digest { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
