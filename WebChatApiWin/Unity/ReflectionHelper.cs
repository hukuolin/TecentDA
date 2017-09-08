using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace WebChatApiWin
{
    public static class ReflectionHelper
    {
        public static string GetPropertyString<T>(this T obj,string property) where T:class
        {
            Type t = obj.GetType();
            PropertyInfo pi= t.GetProperty(property);
            if (pi == null) { return null; }
            object data = pi.GetValue(obj, null);
            return data == null ? string.Empty : data.ToString();
        }
    }
}
