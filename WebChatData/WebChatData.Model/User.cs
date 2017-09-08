using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebChatData.Model
{
    public class User : BaseUser
    {
        public string WebWxPluginSwitch { get; set; }
        public int HeadImgFlag { get; set; }
       
    }
    public class Loginer 
    {
        public User User { get; set; }
    }
    /// <summary>
    /// 微信用户公用字段
    /// </summary>
    public class BaseUser 
    {
        public string Uin { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string HeadImgUrl { get; set; }
        public string RemarkName { get; set; }
        /// <summary>
        /// 拼音首字母大写
        /// </summary>
        public string PYInitial { get; set; }
        /// <summary>
        /// 拼音全拼
        /// </summary>
        public string PYQuanPin { get; set; }
        /// <summary>
        /// 备注拼音首字母
        /// </summary>
        public string RemarkPYInitial { get; set; }
        /// <summary>
        /// 备注全拼
        /// </summary>
        public string RemarkPYQuanPin { get; set; }
        public int HideInputBarFlag { get; set; }
        public int StarFriend { get; set; }
        public int Sex { get; set; }//2女 
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
        public string AppAccountFlag { get; set; }
        public int VerifyFlag { get; set; }
        public string ContactFlag { get; set; }
        public int SnsFlag { get; set; }
    }
}
