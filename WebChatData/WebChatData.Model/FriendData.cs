using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebChatData.Model
{

    public class FriendData : BaseUser
    {
        
        public int MemberCount { get; set; }
       
        public int OwnerUin { get; set; }
   
        public int Statues { get; set; }
        public string AttrStatus { get; set; }
        public string Province { get; set; }//所在省份
        public string City { get; set; }//城市
        /// <summary>
        /// 微信id
        /// </summary>
        public string Alias { get; set; }
        public string AliasDesc { get; set; }
        public int UniFriend { get; set; }
        public string DisplayName { get; set; }
        public int ChatRoomId { get; set; }
        public string KeyWord { get; set; }
        public string EncryChatRoomId { get; set; }
        public int IsOwner { get; set; }
        public string OrderByName
        {
            get
            {
                if (string.IsNullOrEmpty(RemarkName)) { return NickName; }
                return RemarkName;
            }
        }
        /// <summary>
        /// 非微信自带关键字【程序】
        /// </summary>
        [NotMapped]
        public string SelfKeyWord { get;private set; }
        public void InitKeyword()
        {
            SelfKeyWord = NickName + RemarkName + RemarkPYInitial + Alias + RemarkPYQuanPin;
        }
    }
    public enum FriendDataCategory
    {
        [Description("未知")]
        Unknow = -1,
        [Description("未初始化")]
        UnInit = 0,
        [Description("好友")]
        Friend = 1,
        [Description("群")]
        Group = 2,
        [Description("公众号")]
        PublicAccount = 3
    }
    public class Friend : FriendData
    {
        private string _GroupSign;
        public string GroupSign
        {
            get
            {
                InitData();
                return _GroupSign;
            }
            set { _GroupSign = value; }
        }
        private void InitData()
        {
            if (UserName.Contains("@@"))
            {//这是群
                _SelfDefineType = FriendDataCategory.Group;
                _GroupSign = "【群】";
                return;
            }
            if (VerifyFlag != 0 && VerifyFlag % 8 == 0)
            {
                _SelfDefineType = FriendDataCategory.PublicAccount;
                _GroupSign = "【公众号】";
                return;
            }
            _SelfDefineType = FriendDataCategory.Friend;
            _GroupSign = "【好友】";
        }
        private FriendDataCategory _SelfDefineType;
        public FriendDataCategory SelfDefineType
        {
            get
            {
                if (_SelfDefineType == FriendDataCategory.UnInit) InitData();
                return _SelfDefineType;
            }
            set { _SelfDefineType = value; }
        }
    }
    /// <summary>
    /// 用户微信好友数据分析的实体类
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("WebChatFriendData")] //table 特性来自于System.ComponentModel.DataAnnotations.Schema
    public class FriendDataDA : Friend
    {
        /// <summary>
        /// 导入到数据库时作为主键
        /// </summary>
        
       // [NotMapped]//设置不匹配字段 EntityFramework  System.ComponentModel.DataAnnotations.Schema
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 好友所属【ID】
        /// </summary>
        public string DataBelongUserName { get; set; }
        /// <summary>
        /// 好友所属【昵称】
        /// </summary>
        public string DataBelongUserNick { get; set; }
        /// <summary>
        /// 系统提供默认数据标志位时间戳 年月日小时
        /// </summary>
        public string SelfDefineDataTag { get; set; }
        public void Init() 
        {
            Id = Guid.NewGuid();
            CreateTime = DateTime.Now;
            SelfDefineDataTag = CreateTime.ToString("yyyyMMddHHmmss");
        }
    }
}
