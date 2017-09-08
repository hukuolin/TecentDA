using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebChatData.IDAService
{
    public interface IBaseService<T> where T:class
    {
        T Get(object Id);
        bool Update(T entity);
        bool Delete(object Id);
        bool Add(T entity);
    }
}
