using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebChatData.Model;
using WebChatData.IDAService;
namespace WebChatData.MssqlDAService
{
    public class WebFriendService:IWebFriendService
    {
        string connstring;
        public WebFriendService(string appSectionOrConnString)
        {
            connstring = appSectionOrConnString;
        }
        MainRespority<FriendDataDA> mdb;
        public FriendDataDA Get(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(FriendDataDA entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Add(FriendDataDA entity)
        {

            throw new NotImplementedException();
        }

        public bool InsertList(List<FriendDataDA> list)
        {
            mdb = new MainRespority<FriendDataDA>(connstring);
            return mdb.InsertList(list);
        }
    }
}
