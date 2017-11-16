using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebChatData.IDAService
{
    public interface IWebChatMsgService<T>:IBaseService<T> where T:class
    {
    }
}
