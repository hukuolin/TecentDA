using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
namespace WebChatApiWin
{
    public static class JsonHelper
    {
        public static T ConvertJson<T>(this string json) where T :class
        {
            DataContractJsonSerializer jss = new DataContractJsonSerializer(typeof(T));
            byte[] data = Encoding.UTF8.GetBytes(json);
            MemoryStream ms = new MemoryStream(data);
            T entity = jss.ReadObject(ms) as T;
            return entity;
        }
    }
}
