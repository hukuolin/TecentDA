using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
namespace WebChatApiWin
{
    public class HttpClientExt
    {
       
        public static string RunGet(string url) 
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response= client.GetAsync(url).Result;
            //提取cookie
            HttpResponseHeaders msgH = response.Headers;
            string msg = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            client.Dispose();
            return msg;
        }
        public static string RunPost(string url,string formData) 
        {
            string result = string.Empty;
            HttpClient client = new HttpClient();
            HttpContent content=new StringContent(formData,Encoding.UTF8,"application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            result = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            client.Dispose();
            return result;
        }
    }
}
