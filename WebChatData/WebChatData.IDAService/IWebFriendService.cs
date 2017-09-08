using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebChatData.Model;
namespace WebChatData.IDAService
{
    public interface IWebFriendService:IBaseService<FriendDataDA>
    {
        bool InsertList(List<FriendDataDA> list); 
    }
}
