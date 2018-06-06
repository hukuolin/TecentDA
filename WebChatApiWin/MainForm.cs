using FluorineFx.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Configuration;
using Window.DataHelper;
using WebChatData.Model;
using DataHelp;
using WebChatData.MssqlDAService;
using WebChatData.IDAService;
using DataHelp;
using Common.Data;
using Domain.CommonData;
using Infrastructure.ExtService;
using DataHelp;
using QuartzJobService;
using Infrastructure.ExtService;
namespace WebChatApiWin
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 好友列表数据
        /// </summary>
        FriendDataResponse friendList = new FriendDataResponse();
        /// <summary>
        /// 发送消息失败列表
        /// </summary>
        List<Friend> sengMsgFailList = new List<Friend>();
        /// <summary>
        /// 当前登录者
        /// </summary>
        Friend mySelf = new Friend();
        private Dictionary<string, string> _GenderDesc;
        bool UseSqlDA = ConfigurationManager.AppSettings["UseSqlDA"] == "true" ? true : false;
        string GetLoginVerifyCodeUrl = string.Empty;
        string Uin = string.Empty;
        Dictionary<string, string> webChatSampleCfg = new Dictionary<string, string>();
        protected Dictionary<string, string> GenderDes
        {
            get
            {
                if (_GenderDesc == null || _GenderDesc.Count == 0)
                {
                    _GenderDesc = new Dictionary<string, string>();
                    string xml = AppDomain.CurrentDomain.BaseDirectory + "AppSettings.xml";
                    List<Dictionary<string, string>> items = XmlDocumentDataHelper.ReadXmlNodeItem(xml, MapCommonXmlNodeConfigInApp("GenderEnum", "GenderdNodePrimaryKeyValue"));
                    foreach (Dictionary<string, string> item in items)
                    {
                        string[] values = item.Values.ToArray();
                        if (!_GenderDesc.ContainsKey(values[0]) && values.Length >= 2)
                        {
                            _GenderDesc.Add(values[0], values[1]);
                        }
                    }
                }
                return _GenderDesc;
            }
            set { _GenderDesc = value; }
        }
        List<Dictionary<string, string>> columnData;
        FriendDataResponse data = new FriendDataResponse();
        protected List<Dictionary<string, string>> ColumnData
        {
            get
            {
                if (columnData == null || columnData.Count == 0)
                {
                    columnData = InitShowColumnInfo(); ;

                }
                return columnData;
            }
            set { columnData = value; }
        }
        public enum ActionTag
        {
            Common = 0,
            GenerateCodeImage = 1,//生成二维码
            WebChatLogin = 2,//登录微信
            SearchFriendList = 3,//获取好友列表
            SendMsg = 4//发送消息
        }
        string format = ConfigurationManager.AppSettings["DateTimeFormat"];

        enum ControlCategory
        {
            Common = 0,
            Login = 1,
            ShowList = 2,
            SendMsg = -1,
        }
        public MainForm()
        {
            InitializeComponent();
            InitShowElement(ControlCategory.Login.ToString(), true);
            webChatSampleCfg = XmlDocumentDataHelper.GetWebChatCfg();
            InitColumn(lstFriendData);
            InitColumn(lstSelectFriend);
            jslogin();

            qrcode();

            login();
        }
        void InitLoginImage()
        {
            lstProcess.Items.Add(DateTime.Now.ToString(format) + "登录生成二维码");
            InitShowElement(ControlCategory.Login.ToString());
            jslogin();
            qrcode();
            login();
        }
        /// <summary>
        /// 读取xml节点需要的信息
        /// </summary>
        /// <param name="nodeElement">节点所属项标签</param>
        /// <param name="nodeKey">节点的关键检索项的名称</param>
        /// <param name="nodeValue">节点检索项的值</param>
        /// <returns></returns>
        string MapXmlNodeConfigInApp(string nodeElement, string nodeKey, string nodeValue)
        {
            return ConfigurationManager.AppSettings[nodeElement] + "[@" + ConfigurationManager.AppSettings[nodeKey] + "=\"" + ConfigurationManager.AppSettings[nodeValue] + "\"]";
        }
        /// <summary>
        /// 公用关键列的节点检索【关键列配置与app.config中】
        /// </summary>
        /// <param name="nodeElement"></param>
        /// <param name="nodeValue"></param>
        /// <returns></returns>
        string MapCommonXmlNodeConfigInApp(string nodeElement, string nodeValue)
        {
            return ConfigurationManager.AppSettings[nodeElement] + "[@" + ConfigurationManager.AppSettings["xmlCommonPrimaryKey"] + "=\"" + ConfigurationManager.AppSettings[nodeValue] + "\"]";
        }
        List<Dictionary<string, string>> InitShowColumnInfo()
        {
            string xml = AppDomain.CurrentDomain.BaseDirectory + "/WebChatFriend.xml";

            string node = MapXmlNodeConfigInApp("WebChatFriendNode", "WebChatFriendNodePrimaryKey", "WebChatFriendNodePrimaryKeyValue");
            //  ConfigurationManager.AppSettings["WebChatFriendNode"] + "[@" + ConfigurationManager.AppSettings["WebChatFriendNodePrimaryKey"] + "=\"" + ConfigurationManager.AppSettings["WebChatFriendNodePrimaryKeyValue"] + "\"]";
            List<Dictionary<string, string>> items = XmlDocumentDataHelper.ReadXmlNodeItem(xml, node);
            return items;
        }
        private void login()
        {
            ShowMsg("login.....");
            if (__3)
            {
                //第三步，等待扫描
                var time = new System.Windows.Forms.Timer();
                time.Interval = sleeptime;
                time.Stop();

                int count = 0;
                time.Tick += new EventHandler(delegate
                {
                    lstProcess.Items.Add(txtFriendFilter.Enabled);
                    if (count++ > login_try_times)
                    {
                        time.Stop();
                        txtTip.Text = "错误的次数超过了:" + login_try_times + "次";
                        //time.Enabled = false;
                        //time.Stop();
                        return;
                    }

                    SendHeader(httpclient, url_login[1]);
                    string getLoginKeyUrl = ReplaceKey(url_login[0]);
                    System.Threading.Tasks.Task<HttpResponseMessage> msg = httpclient.GetAsync(getLoginKeyUrl);
                    //提取cookie

                    HttpResponseMessage hrm = msg.Result;
                    var task = httpclient.GetStringAsync(getLoginKeyUrl);
                    var result = task.Result;

                    if (result.IndexOf("window.redirect_uri=") != -1)
                    {
                        time.Stop();
                        redirect_uri = GetResultString(result, "\"(.*?)\"");
                        ///校验返回内容是否为登录凭据
                        GetLoginVerifyCodeUrl = redirect_uri;//此时尚未获取到登陆的cookie信息
                        // GetVerifyKey(redirect_uri);
                        // string lisence= httpclient.GetStringAsync(redirect_uri).Result;//直接使用重定向的URL进行访问时提示浏览器版本过低
                        if (redirect_uri.IndexOf("wx2.qq.com") != -1)
                            WXNUMBER = "2";

                        //执行第四部
                        doStep4();
                    }
                });
                time.Start();
            }
        }

        private void qrcode()
        {
            ShowMsg("qrcode");
            if (__2)
            {
                SendHeader(httpclient, url_qrcode[1]);
                var task = httpclient.GetStreamAsync(ReplaceKey(url_qrcode[0]));

                var result = task.Result;
                this.pictureBox1.BackgroundImage = Image.FromStream(result);
            }
        }

        private void jslogin()
        {
            ShowMsg("islogin");
            if (__1)
            {
                SendHeader(httpclient, url_jslogin[1]);
                // var task = httpclient.GetStreamAsync(ReplaceKey(url_jslogin[0]));
                var result = HttpClientExt.RunGet(ReplaceKey(url_jslogin[0]));
                // var result = GetDeflateByStream(task.Result, "GBK");
                UUID = GetCodeString(result, "200", "\"(.*?)\"");


            }
        }


        private void doStep4()
        {
            InitShowElement(ControlCategory.SendMsg.ToString());
            //this.pictureBox1.Visible = false;
            //this.lstProcess.Visible = true;
            //this.btnSend.Visible = true;
            //this.txtBoxMessage.Visible = true;
            //this.btnGetUserList.Visible = true;

            redirect_uri_fun();

            webwxinit_new();

            webwxgetcontact();

            //循环检查状态，如果得到了最新信息，然后开始执行
            synccheck();

            urldownload();

            //开启一个timer，一直给我发送信息
            var timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += (o, e) =>
            {
                if (!bFirst)
                {
                    timer.Interval = 10 * 60 * 1000;
                    bool b = false;
                    foreach (var s in USER_DI)
                    {
                        if (s.Value.IndexOf("都好") != -1)
                        {
                            b = true;
                            SendMsg(s.Key, USER_INFO, DateTime.Now.ToString(),Uin, false);
                            break;
                        }
                    }

                    if (b == false)
                    {
                        //如果走到这一步，就随机一个人发
                        //  SendMsg(USER_DI.ElementAt((new Random()).Next(USER_DI.Count())).Key, USER_INFO,DateTime.Now.ToString(), false);
                    }
                }
            };

            timer.Start();
        }


        bool bFirst = true;
        private void redirect_uri_fun()
        {
            ShowMsg("redirect_uri_fun");
            if (__4)
            {
                HttpWebRequest h = (HttpWebRequest)HttpWebRequest.Create(redirect_uri);
                h.AllowAutoRedirect = false;
                h.CookieContainer = cookieContainer;
                HttpWebResponse r = (HttpWebResponse)h.GetResponse();
                //此时进行cookie提取
               // CookieCollection cookies = r.Cookies;
               //// cookieContainer.Add(cookies);
               // foreach (Cookie item in cookies)
               // {
               //     cookieContainer.Add(new Cookie(item.Name, item.Value,item.Path,item.Domain));
               // }
                COOKIES = GetAllCookiesA(cookieContainer);//  'webwxuvid'  'webwx_auth_ticket'  'wxuin' 'mm_lang' 'wxloadtime' 五项  cookie
                // 然而实际的请求需要项  'pgv_pvi' 'webwxuvid' 'webwx_auth_ticket'  'wxloadtime'  'wxpluginkey'  'wxuin'  'mm_lang'
                #region 登录成功就查询相关信息
                string loginIdUrl = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxinit?r=$r$ ";
                loginIdUrl = loginIdUrl.Replace("$r$", new TestClass().GenerateJsNegate());
                RequestHttp(loginIdUrl, cookieContainer);
                #endregion

                using (System.IO.StreamReader read = new System.IO.StreamReader(r.GetResponseStream()))
                {
                    string value = read.ReadToEnd();
                    if (value.IndexOf("pass_ticket") == -1) throw new Exception("没有得到wxsid信息");
                    step4xml = Xml2Json<Step4XML>(value);
                }
                r.Close();
                if (!string.IsNullOrEmpty(GetLoginVerifyCodeUrl)&&GetLoginVerifyCodeUrl.Contains("webwxnewloginpage"))//登陆成功之后获取登录者的登录tocken
                {
                    WebChatLoginTocken tocken = GetLoginTocken(GetLoginVerifyCodeUrl, DeviceID);
                    if (tocken == null)
                    {
                        return;
                    }
                    //开启新线程进行调度
                    AsyncThreadDoEvent async = new Infrastructure.ExtService.AsyncThreadDoEvent();
                    async.OpenNewThreadWithRun(new AsyncThreadDoEvent.CallEvent(NewThreadDoQueryMsg), new object[] { tocken, DeviceID, cookieContainer });
                }
                /*
                     https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxnewloginpage?ticket=A-UlGlrnWCqys2myqayFNghP@qrticket_0&uuid=oY-3-4EsAg==&lang=zh_CN&scan=1510751386&fun=new&version=v2&lang=zh_CN
                     */
               
                // GetVerifyKey(GetLoginVerifyCodeUrl, DeviceID);
            }
        }
        public void RequestHttp(string url, CookieContainer cookieContainer)
        {
            HttpWebRequest h = (HttpWebRequest)HttpWebRequest.Create(url);
            h.AllowAutoRedirect = false;
            h.CookieContainer = cookieContainer;
            h.Accept = "application/javascript, */*;q=0.8";
            HttpWebResponse r = (HttpWebResponse)h.GetResponse();
            using (System.IO.StreamReader read = new System.IO.StreamReader(r.GetResponseStream()))
            {
                string value = read.ReadToEnd();
                new TestClass().WriteLoginCookie(value);
            }
        }
        private void synccheck()
        {
            ShowMsg("synccheck");
            if (__6)
            {
                var urlA = "https://webpush[number].weixin.qq.com/cgi-bin/mmwebwx-bin/synccheck?r={time}&skey={SKEY}&sid={SID}&uin={UIN}&deviceid={DeviceID}&synckey={synckey}&_={time}";

                bool bRun = true;

                ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                {
                    while (bRun)
                    {
                        Thread.Sleep(2000);
                        try
                        {
                            var url = ReplaceKey(urlA);
                            if (bFirst) ShowMsg("正在解析中...等待!");
                            JavaScriptObject obj = JavaScriptConvert.DeserializeObject(SyncKey) as JavaScriptObject;
                            JavaScriptArray list = obj["List"] as JavaScriptArray;
                            var k = "";
                            foreach (JavaScriptObject o in list)
                                k += "|" + o["Key"] + "_" + o["Val"];

                            url = url.Replace("{synckey}", k.Substring(1));

                            HttpWebRequest h = (HttpWebRequest)HttpWebRequest.Create(url);
                            h.AllowAutoRedirect = false;
                            h.CookieContainer = cookieContainer;
                            h.Accept = "application/javascript, */*;q=0.8";
                            HttpWebResponse r = (HttpWebResponse)h.GetResponse();
                            using (System.IO.StreamReader read = new System.IO.StreamReader(r.GetResponseStream()))
                            {

                                string value = read.ReadToEnd();
                                if (value.Contains("1101"))
                                {
                                    bRun = false;

                                    ShowMsg("请重新打开!" + url);
                                }
                                else
                                {
                                    if (bFirst)
                                    {
                                        bFirst = false;
                                        ShowMsg("正常运行!");
                                    }
                                    Console.WriteLine("1=>" + value);
                                    //string ret = value;
                                    //window.synccheck={retcode:"0",selector:"6"}
                                    if (value.IndexOf("selector:\"0\"") == -1 && value.IndexOf("retcode:\"0\"") != -1)
                                        doStep7();
                                }

                            }
                            r.Close();
                        }
                        catch
                        {
                        }
                    }
                }));
            }
        }



        private void webwxgetcontact()
        {
            ShowMsg("webwxgetcontact");
            if (true)
            {
                var url = "https://wx[number].qq.com/cgi-bin/mmwebwx-bin/webwxgetcontact?pass_ticket={pass_ticket}&r={time}&skey={SKEY}";

                HttpWebRequest h = (HttpWebRequest)HttpWebRequest.Create(ReplaceKey(url));
                h.AllowAutoRedirect = false;
                h.CookieContainer = cookieContainer;
                //提取全部的cookie

                h.Accept = "application/json, text/plain, */*";
                HttpWebResponse r = (HttpWebResponse)h.GetResponse();
                CookieCollection cookies = r.Cookies;

                using (System.IO.StreamReader read = new System.IO.StreamReader(r.GetResponseStream()))
                {
                    string value = read.ReadToEnd();
                    USER_LIST = JavaScriptConvert.DeserializeObject(value) as JavaScriptObject;

                    //显示到list中
                    var arr = USER_LIST["MemberList"] as JavaScriptArray;
                    DataContractJsonSerializer jss = new DataContractJsonSerializer(typeof(FriendDataResponse));

                    data = value.ConvertJson<FriendDataResponse>();
                    friendList = data;


                    foreach (JavaScriptObject o in arr)
                    {
                        USER_DI[o["UserName"] + ""] = o["NickName"] + "";
                        //NickName
                    }
                }
                r.Close();
                bool onlyFriend = ckOnlyFriend.Checked;
                List<Friend> friends = new List<Friend>();
                IEnumerable<Friend> ef;
                if (onlyFriend)
                    ef = data.MemberList.Where(m =>
                    {
                        m.AliasDesc = m.UserName.Replace("@", "").Convert16ToString();
                        return m.SelfDefineType == FriendDataCategory.Friend;
                    });
                else
                    ef = data.MemberList;
                friends = ef.OrderBy(s => s.OrderByName).ToList();
                // data.MemberList.Sort(data.MemberList)
                ListBindDataSource(friends);
            }
        }

        private void webwxinit_new()
        {
            ShowMsg("webwxinit_new");
            if (__5)
            {
                SendHeader(httpclient, ReplaceHeaderKey(url_webwxinit[1]));
                byte[] bs = Encoding.UTF8.GetBytes(ReplaceHeaderKey(@"{""BaseRequest"":{""Uin"":""{UIN}"",""Sid"":""{SID}"",""Skey"":""{SKEY}"",""DeviceID"":""{DeviceID}""}}"));
                string json = ReplaceHeaderKey(@"{""BaseRequest"":{""Uin"":""{UIN}"",""Sid"":""{SID}"",""Skey"":""{SKEY}"",""DeviceID"":""{DeviceID}""}}");
                string url = ReplaceKey(url_webwxinit[0]);

                //var task = httpclient.PostAsync(ReplaceKey(url_webwxinit[0]), new ByteArrayContent(bs));
                {
                    //  string value = GetDeflateByStream(task.Result.Content.ReadAsStreamAsync().Result);
                    string value = HttpClientExt.RunPost(url, json);
                    //pick up loginer
                    Loginer loginer = value.ConvertJson<Loginer>();
                    BaseUser me = loginer.User.ConvertMapModel<User, BaseUser>();
                    mySelf = me.ConvertMapModel<BaseUser, Friend>();
                    mySelf.IsOwner = 1;
                    //"Ret": 1100,
                    if (!value.Contains("\"Ret\": 0"))
                    {
                        ShowMsg("没有返回正确的数据，webwxinit错误!");
                        // throw new Exception("没有返回正确的数据，webwxinit错误!");
                    }

                    //USER_INFO
                    USER_INFO = SubString(value.Replace("\r", "").Replace("\n", ""), "\"User\": {", "NickName");
                    USER_INFO = SubString(USER_INFO, "\"UserName\": \"", "\",");


                    USER_NICKNAME = SubString(value.Replace("\r", "").Replace("\n", ""), "\"User\": {", "HeadImgUrl");
                    USER_NICKNAME = SubString(USER_NICKNAME, "\"NickName\": \"", "\",");


                    label1.Text = USER_INFO;
                    USER_DI.Add(USER_INFO, USER_NICKNAME);

                    this.Text = USER_NICKNAME + ">>>转发微信机器人 V0.5.1 20170502";

                    //SyncKey
                    SyncKey = SubString(value.Replace("\r", "").Replace("\n", ""), "\"SyncKey\": ", "}]}");
                    SyncKey += "}]}";
#if DEBUG
                    this.txtTip.Text = SyncKey;
#endif
                }
            }
        }

        /// <summary>
        /// 发送文件
        /// </summary>
        private void btnSendFile_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                sendFileMsg(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="filePath"></param>
        void sendFileMsg(string filePath)
        {
            HttpClient newClient = new HttpClient();
            {
                FileStream fs = File.OpenRead(filePath);

                var requestContent = new MultipartFormDataContent();
                string webwxuploadmedia1bodyUrl = ReplaceHeaderKey(webwxuploadmedia1body2);
                ByteArrayContent txtContent = new ByteArrayContent(Encoding.UTF8.GetBytes(webwxuploadmedia1bodyUrl.Replace("[CD]", fs.Length + "")));
                txtContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "uploadmediarequest"
                };
                requestContent.Add(txtContent);
                Dictionary<string, string> di = new Dictionary<string, string>();
                di["id"] = "WU_FILE_0";
                di["name"] = "webwxgetvoice.mp3";
                di["type"] = "audio/mpeg";
                di["size"] = fs.Length + "";
                di["mediatype"] = "doc";
                di["webwx_data_ticket"] = COOKIES["webwx_data_ticket"];
                di["pass_ticket"] = this.step4xml.pass_ticket;
                foreach (string s in di.Keys)
                {
                    txtContent = new ByteArrayContent(Encoding.UTF8.GetBytes(di[s]));
                    txtContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = s
                    };
                    requestContent.Add(txtContent);
                }



                //var imageContent = new ByteArrayContent(temp);
                //imageContent.Headers.ContentType =
                //    MediaTypeHeaderValue.Parse("audio/mpeg");                
                //requestContent.Add(imageContent, "filename","webwxgetvoice.mp3");
                var t = new StreamContent(fs);
                t.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/mp3");//application/octet-stream
                requestContent.Add(t, "filename", "webwxgetvoice.mp3");

                SendHeader(newClient, webwxuploadmedia1[1]);

                var task = newClient.PostAsync(ReplaceHeaderKey(webwxuploadmedia1[0]), requestContent);
                var value = task.Result.Content.ReadAsStringAsync().Result;
                Console.WriteLine(value);

                //requestContent.Add(imageContent, "image", "image.jpg");
            }

        }

        private void webwxinit_old()
        {
            ShowMsg("webwxinit_old");
            if (__5)
            {
                HttpWebRequest h = (HttpWebRequest)HttpWebRequest.Create(ReplaceKey(url_webwxinit[0]));
                h.AllowAutoRedirect = false;
                h.CookieContainer = cookieContainer;
                h.Method = "POST";
                h.Accept = "application/json, text/plain, */*";
                h.ContentType = "application/json;charset=utf-8";

                byte[] bs = Encoding.UTF8.GetBytes(ReplaceHeaderKey(@"{""BaseRequest"":{""Uin"":""{UIN}"",""Sid"":""{SID}"",""Skey"":""{SKEY}"",""DeviceID"":""{DeviceID}""}}"));
                using (Stream reqStream = h.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Close();
                }
                HttpWebResponse r = (HttpWebResponse)h.GetResponse();


                using (System.IO.StreamReader read = new System.IO.StreamReader(r.GetResponseStream()))
                {
                    string value = read.ReadToEnd();
                    //"Ret": 1100,
                    if (!value.Contains("\"Ret\": 0"))
                    {
                        ShowMsg("没有返回正确的数据，webwxinit错误!");
                        throw new Exception("没有返回正确的数据，webwxinit错误!");
                    }

                    //USER_INFO
                    USER_INFO = SubString(value.Replace("\r", "").Replace("\n", ""), "\"User\": {", "NickName");
                    USER_INFO = SubString(USER_INFO, "\"UserName\": \"", "\",");

                    label1.Text = USER_INFO;
                    USER_DI.Add(USER_INFO, "我自己");

                    //SyncKey
                    SyncKey = SubString(value.Replace("\r", "").Replace("\n", ""), "\"SyncKey\": ", "}]}");
                    SyncKey += "}]}";
#if DEBUG
                    this.txtTip.Text = SyncKey;
#endif
                }
                r.Close();


            }
        }


        //https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxgetvoice?msgid=8332473900244757099&skey=@crypt_39864b32_3b5756470b541ab03f03f519c0f1d2a9

        void UploadWxImage(string MsgID, string Form, string FormName, string ad)
        {
            try
            {
                //下载图片
                //https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxgetmsgimg?&MsgID=8055351800675473074&skey=%40crypt_671d6fec_1cd01b65296c06ef559ae98b0646725a&type=slave
                //image/png,image/*;q=0.8,*/*;q=0.5
                //&type=slave 获取缩略图的意思
                Image img = DownLoadImage(MsgID);

                //SendImage(Form, FormName, ad, img);

            }
            catch (Exception err)
            {
#if DEBUG
                SendMsg(Form, USER_INFO, err.Message,Uin, false);
#else
                SendMsg(Form, USER_INFO, "接收图片出现错误!",Uin, false);
#endif
            }
        }

        private Image DownLoadImage(string MsgID, string slave = "&type=slave")
        {
            var url = "https://wx[number].qq.com/cgi-bin/mmwebwx-bin/webwxgetmsgimg?&MsgID={0}&skey={1}" + slave;


            HttpWebRequest h = (HttpWebRequest)HttpWebRequest.Create(ReplaceHeaderKey(string.Format(url, MsgID, step4xml.skey)));
            h.AllowAutoRedirect = false;
            h.CookieContainer = cookieContainer;
            h.Accept = "image/png,image/*;q=0.8,*/*;q=0.5";
            HttpWebResponse r = (HttpWebResponse)h.GetResponse();
            Image img = Image.FromStream(r.GetResponseStream());
            r.Close();
            return img;
        }

        //https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxgetvoice?msgid=8332473900244757099&skey=@crypt_39864b32_3b5756470b541ab03f03f519c0f1d2a9
        //private Image DownLoadvoice(string MsgID)
        //{
        //    var url = "https://wx[number].qq.com/cgi-bin/mmwebwx-bin/webwxgetvoice?&msgid={0}&skey={1}";


        //    HttpWebRequest h = (HttpWebRequest)HttpWebRequest.Create(ReplaceHeaderKey(string.Format(url, MsgID, step4xml.skey)));
        //    h.AllowAutoRedirect = false;
        //    h.CookieContainer = cookieContainer;
        //    //h.Accept = "image/png,image/*;q=0.8,*/*;q=0.5";
        //    HttpWebResponse r = (HttpWebResponse)h.GetResponse();
        //    MemoryStream ms = new MemoryStream(r.GetResponseStream());
        //    r.Close();
        //    return ms;
        //}



        void doStep7()
        {
            if (__7)
            {
                //https://webpush.weixin.qq.com/cgi-bin/mmwebwx-bin/synccheck?r=1444004057053&skey=%40crypt_39864b32_c5aaad7d38892d44fde5da7e97b32e69&sid=iM3uR2da1I2t0Upy&uin=840648442&deviceid=e013657496826621&synckey=1_634979925%7C2_634981902%7C3_634981851%7C11_634981845%7C201_1444003994%7C1000_1443952994&_=1444003858911
                //var url = "https://webpush.weixin.qq.com/cgi-bin/mmwebwx-bin/synccheck?r={time}&skey={SKEY}&sid={SID}&uin={UIN}&deviceid={DeviceID}&synckey=1_634979925%7C2_634981902%7C3_634981851%7C11_634981845%7C201_1444003994%7C1000_1443952994&_=1444003858911

                //var url = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxsync?sid={SID}&r={time}&skey={SKEY}";
                var url = "https://wx[number].qq.com/cgi-bin/mmwebwx-bin/webwxsync?sid={SID}&skey={SKEY}&lang=zh_CN&pass_ticket={pass_ticket}";

                HttpWebRequest h = (HttpWebRequest)HttpWebRequest.Create(ReplaceKey(url));
                h.AllowAutoRedirect = false;
                h.CookieContainer = cookieContainer;
                h.Method = "POST";
                h.Accept = "application/json, text/plain, */*";
                h.ContentType = "application/json;charset=utf-8";

                byte[] bs = Encoding.ASCII.GetBytes(ReplaceHeaderKey(@"{""BaseRequest"":{""Uin"":{UIN},""Sid"":""{SID}"",""Skey"":""{SKEY}"",""DeviceID"":""{DeviceID}""},""SyncKey"":{SyncKey},""rr"":{time}}"));
                using (Stream reqStream = h.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Close();
                }

                HttpWebResponse r = (HttpWebResponse)h.GetResponse();

                //var list = GetAllCookies(cookieContainer);

                using (System.IO.StreamReader read = new System.IO.StreamReader(r.GetResponseStream()))
                {
                    string value = read.ReadToEnd();

                    if (value.IndexOf("\"SyncKey\": ") == -1) throw new Exception("SyncKey 没有捕获到");

                    SyncKey = SubString(value.Replace("\r", "").Replace("\n", ""), "\"SyncKey\": ", "}]}");
                    SyncKey += "}]}";

                    //得到用户的消息
                    JavaScriptObject ret = JavaScriptConvert.DeserializeObject(value) as JavaScriptObject;
                    JavaScriptArray arr = ret["AddMsgList"] as JavaScriptArray;
                    foreach (JavaScriptObject obj in arr)
                    {
                        var content = obj["Content"].ToString().Replace("&gt;", ">").Replace("&lt;", "<").Replace("<br/>", "");
                        ShowMsg(content, obj);

                        //处理消息
                        DoMsg(content, obj, obj["MsgId"] + "");
                    }
                }
                r.Close();
            }
        }

        void ShowMsg(string msg, JavaScriptObject obj = null)
        {
            DelegateFun.ExeControlFun(lstProcess, new DelegateFun.delegateExeControlFun(delegate
            {
                //全部清除
                if (lstProcess.Items.Count > 3000) lstProcess.Items.Clear();

                Console.WriteLine("2=>" + msg);
                lstProcess.Items.Add(DateTime.Now + ">2=>" + msg);
                if (obj != null)
                {
                    var _FormUserName = obj["FromUserName"] + "";
                    var _ToUserName = obj["ToUserName"] + "";
                    var FormUserName = GetDIName(_FormUserName);
                    var ToUserName = GetDIName(_ToUserName);

                    lstProcess.Items.Add(DateTime.Now + ">" + FormUserName + ">" + ToUserName + ":" + msg);

                    this.lstProcess.TopIndex = this.lstProcess.Items.Count - (int)(this.lstProcess.Height / this.lstProcess.ItemHeight);
                }

                //listBox1.

            }));
        }

        /// <summary>
        /// 给用户发送信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            string[] select = PickUpSelectFriend();

            object user = select.Length > 0 ? select[0] : null;
            // object user = lstBoxUser.SelectedItem;
            if (user == null)
            {
                txtTip.Text = "请选择用户！";
                return;
            }

            if (string.IsNullOrEmpty(txtBoxMessage.Text.Trim()) && !ckAppendDateTime.Checked)
            {
                txtTip.Text = "请输入信息！";
                return;
            }
            //是否可以批量发送呢？



            //string userid = user.ToString().Substring(user.ToString().LastIndexOf('>')+1);

            //string userid = string.Join(",", select);// user.ToString();
            foreach (string item in select)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(txtBoxMessage.Text);
                if (ckAppendDateTime.Checked)
                {
                    sb.AppendLine(DateTime.Now.ToString(format));
                }
                SendMsgResponse result= SendMsg(item, user.ToString(), sb.ToString(),Uin, false);
                txtTip.Text += "\r\n" + result.BaseResponse.ErrMsg;
               // SendMsg(item, USER_INFO, sb.ToString(), false);
            }
            if (ckClearText.Checked)
            {
                txtBoxMessage.Text = "";
            }
        }

        public string USER_NICKNAME { get; set; }

        private void btnGetUserList_Click(object sender, EventArgs e)
        {
            webwxgetcontact();
        }



        public Dictionary<string, string> COOKIES { get; set; }

        void ListBindDataSource(List<Friend> members)
        {
            //程序运行后的路径获取

            List<string> columneName = new List<string>();
            foreach (ColumnHeader item in lstFriendData.Columns)
            {
                if (string.IsNullOrEmpty(item.Name))
                {
                    continue;
                }
                columneName.Add(item.Name);
            }
            lstFriendData.Items.Clear();
            int index = 0;
            foreach (Friend item in members)
            {
                string[] row = new string[columneName.Count];
                for (int i = 0; i < columneName.Count; i++)
                {
                    row[i] = item.GetPropertyString(columneName[i]);
                    if (string.IsNullOrEmpty(row[i]) && i == 0)
                    {
                        row[i] = index.ToString();
                    }
                    if (columneName[i] == "Sex" && GenderDes.ContainsKey(row[i]))
                    {
                        row[i] = GenderDes[row[i]];
                    }
                }
                index++;

                ListViewItem lvi = new ListViewItem(row, columneName.Count);
                lvi.ImageIndex = 0;
                lvi.Tag = item.UserName;
                lstFriendData.Items.Add(lvi);
            }
            if (!UseSqlDA)
            {//不使用数据库进行数据存在
                return;
            }
            Thread th = new Thread(SyncWebChatData);
            members.Add(mySelf);
            th.Start(members);//传递参数
            Thread.Sleep(10 * 1000);//延迟数秒之后执行数据同步
        }
        void InitColumn(ListView lst)
        {
            List<Dictionary<String, string>> items = ColumnData;
            List<ColumnHeader> heads = new List<ColumnHeader>();
            ColumnHeader action = new ColumnHeader();
            action.Name = "Add";
            action.Text = "选择";
            heads.Add(action);
            if (items != null)
            {
                foreach (Dictionary<string, string> item in items)
                {

                    if (item["hidden"] != "true")
                    {
                        ColumnHeader head = new ColumnHeader() { Width = 200 };
                        head.Name = item["key"];
                        head.Text = item["value"];
                        if (string.IsNullOrEmpty(head.Text))
                        {
                            head.Text = head.Name;
                        }
                        heads.Add(head);
                    }
                }
            }
            lst.View = View.Details;
            lst.SmallImageList = imageList;

            //lst.View = View.Details;
            lst.Columns.AddRange(heads.ToArray());
            ListViewExt lstE = new ListViewExt();
            lst.Columns[0].ImageIndex = 0;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView ls = sender as ListView;
            if (ls.SelectedItems.Count == 0) { return; }
            ListViewItem item = ls.FocusedItem;
            if (item == null) { return; }
            if (ls.Name == lstSelectFriend.Name)
            {//
                ls.Items.Remove(item);
                return;
            }
            string tag = item.Tag as string;
            foreach (ListViewItem cc in lstSelectFriend.Items)
            {
                if ((cc.Tag as string) == tag) { return; }
            }
            ListViewItem select = item.Clone() as ListViewItem;
            //select.ImageList
            lstSelectFriend.Items.Add(select);
        }
        /// <summary>
        /// 提取选择进行消息发送的微信好友
        /// </summary>
        /// <returns></returns>
        string[] PickUpSelectFriend()
        {
            ListView.ListViewItemCollection datas = lstSelectFriend.Items;
            if (datas.Count == 0) { return new string[0]; }
            List<string> select = new List<string>();
            foreach (ListViewItem item in datas)
            {
                select.Add(item.Tag as string);
            }
            return select.ToArray();
        }

        private void btnRefreshVC_Click(object sender, EventArgs e)
        {
            InitLoginImage();
        }

        private void btnClearTextprocess_Click(object sender, EventArgs e)
        {
            lstProcess.Items.Clear();
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ck = sender as CheckBox;
            string tag = ck.Tag as string;
            string[] ts = new string[0];
            if (string.IsNullOrEmpty(tag) || (ts = tag.Split('&')).Length > 0) { }
            if (ts.Contains("lstFriendData") && ck.Checked)
            {//选择全部好友
                ListView.ListViewItemCollection all = lstFriendData.Items;
                lstSelectFriend.Items.Clear();
                foreach (ListViewItem item in all)
                {
                    ListViewItem select = item.Clone() as ListViewItem;
                    lstSelectFriend.Items.Add(select);
                }
            }
            else if (ts.Contains("lstSelectFriend") && ck.Checked)
            {//全部移除
                lstSelectFriend.Items.Clear();
            }
        }
        /// <summary>
        /// 初始化显示的元素【第一次初始化界面元素时不控制隐藏的元素进行显示】
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="isFirstInitForm"></param>
        private void InitShowElement(string tag, bool isFirstInitForm = false)
        {
            Control.ControlCollection eles = this.Controls;
            foreach (Control item in eles)
            {
                string tagE = item.Tag as string;
                string[] ts = new string[0];
                if (isFirstInitForm && item.Visible)
                {
                    continue;
                }
                if (string.IsNullOrEmpty(tagE) || (ts = tagE.Split('&')).Length > 0) { }
                if (ts.Length == 0 || ts.Contains(ActionTag.Common.ToString()) || ts.Contains(tag))
                {
                    item.Show();
                }
                else
                {
                    item.Hide();
                }
            }
        }
        private void SyncWebChatData(object obj)
        {
            List<Friend> fs = (List<Friend>)obj;
            FriendDataDA da = new FriendDataDA();
            Friend fri = fs.Where(f => f.IsOwner > 0).FirstOrDefault();
            da.Init();
            List<FriendDataDA> fas = fs.Select<Friend, FriendDataDA>(s => s.ConvertMapModel<Friend, FriendDataDA>())
                .Where(d =>
                {
                    if (fri != null)
                    {
                        d.DataBelongUserNick = fri.NickName;
                        d.DataBelongUserName = fri.UserName;
                    }
                    d.Init();
                    d.CreateTime = da.CreateTime;
                    return true;
                }).ToList();
            IWebFriendService web = new WebFriendService("TencentWebChatDAConnString");
            web.InsertList(fas);
        }
        void QueryGroupMember()
        {
            string skey = step4xml.skey;
            string url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxbatchgetcontact?type=ex&r=1505052926468";
            var request = @"Request URL:https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxbatchgetcontact?type=ex&r=1505052926468
Request Method:POST
Status Code:200 OK
Remote Address:182.254.78.160:443
Referrer Policy:no-referrer-when-downgrade
Response Headers
view source
Connection:keep-alive
Content-Encoding:gzip
Content-Length:7276
Content-Type:text/plain
Request Headers
view source
Accept:application/json, text/plain, */*
Accept-Encoding:gzip, deflate, br
Accept-Language:zh-CN,zh;q=0.8
Connection:keep-alive
Content-Length:8215
Content-Type:application/json;charset=UTF-8
Cookie:tvfe_boss_uuid=1e6199e1d2117b2e; pgv_pvi=2689650688; RK=jY8eVEcaan; eas_sid=D175U044s0B9N5T0M4i788g0p9; ptcz=5574052159393dcaff18034b6297860bf1d967208ee4ad2d010049712f34c236; pt2gguin=o0158055983; ptui_loginuin=3346910365; o_cookie=158055983; pgv_pvid=6691931733; pgv_si=s8835954688; webwxuvid=bdc0ad25db9c3d1d360bdf092781d16b0dd557e9fb0a025e7d60768c394787a8d04bfe28f11ce423a2b2eec1f2f1c53f; webwx_auth_ticket=CIsBENOq5pkOGoABdTmJQY4CbsKm12nphdZ6MrRzNW/dy6xFxxmL+g961XL5D0pmKSpA2QVPa3nYYd9WIcs8QzpVXOdgMSej/B6ko2vWZWbuTn79Ta1uQOnrDjeX0DYzvqYFYbuUbAs71Fxwsx472ImRsScZ6yUylrJ1Agd730kYMwdp7bU8GP58rQI=; wxloadtime=1505052697_expired; mm_lang=zh_CN; MM_WX_NOTIFY_STATE=1; MM_WX_SOUND_STATE=1; wxpluginkey=1505038321; wxuin=2266323382; wxsid=IyK64UOM3Q+ARxUP; webwx_data_ticket=gSdyWtElQHnLQDVNe1B3DP4C
Host:wx.qq.com
Origin:https://wx.qq.com
Referer:https://wx.qq.com/
User-Agent:Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36
Query String Parameters
view source
view URL encoded
type:ex
r:1505052926468
Request Payload
view source
Request Payload
BaseRequest :{
DeviceID:e203048444707618
Sid:IyK64UOM3Q+ARxUP
Skey:@crypt_45ae67ad_b3301bd36372109c8a5e1e149fe4d473
Uin:2266323382
}
Count:50
        List:[
                {
                    EncryChatRoomId:@@7db361ea0fd95ffcbdd04a401cfd6f6c6ce450f838fc498a3d1802150a15c4f7
                    UserName:@0f92446a7d84c941c1b613ad3f0c16a1018663ef6080e960206b88e0070321c6
                }
        ]
";// List 中数据含义  EncryChatRoomId 群组ID，UserName要查询信息的成员标识
        }
        void CallGetVerifyKey(object obj)
        {
            if (lstProcess.InvokeRequired)
            {
                BaseDelegate bd = new BaseDelegate(CallGetVerifyKey);
                this.Invoke(bd, obj);
                // CallGetVerifyKey(obj);
                return;
            }
            object[] o = obj as object[]; 
            GetVerifyKey((WebChatLoginTocken)o[0], (string)o[1],(CookieContainer)o[2]);
        }
        WebChatLoginTocken GetLoginTocken(string loginerVerifyCodeUrl, string deviceId)
        {
            /*
           https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxnewloginpage?ticket=A-UlGlrnWCqys2myqayFNghP@qrticket_0&uuid=oY-3-4EsAg==&lang=zh_CN&scan=1510751386&fun=new&version=v2&lang=zh_CN
             
           */
            loginerVerifyCodeUrl += "&fun=new&version=v2&lang=zh_CN";
            string text = HttpClientExt.RunPosterContainerHeader(loginerVerifyCodeUrl, header, cookieContainer);
            text.CreateLog(ELogType.LogicLog);
            WebChatLoginTocken tocken = new WebChatLoginTocken();
            tocken.GetWebChatLoginTocket(text);
            if (tocken.ret != 0)
            {//实际上登录凭证只需要获取一次即可
                return null;
            }
            return tocken;
        }
        void NewThreadDoQueryMsg(object param) 
        {
            //object[] ps = param as object[];
            //WebChatLoginTocken tocken = ps[0] as WebChatLoginTocken;
            //string deviceID = ps[1] as string;
            //object[] funParam = new object[] { tocken, deviceID };

            QuartzJob job = new QuartzJob();
            BaseDelegate bd = new BaseDelegate(CallGetVerifyKey);
            object[] quartParam = new object[] { bd, param };
            job.CreateJobWithParam<JobDelegate<DADefineHelper>>(quartParam, DateTime.Now, 2, -1);
        }
        void GetVerifyKey(WebChatLoginTocken tocken, string deviceId, CookieContainer cookieContainer)
        {//此处需要一个定时作业进行消息管理
          
            //将数据填入到查询接口中
            string queryMsgUrlFormat = webChatSampleCfg["QueryWebChatMsgUrlPost"];
            string msgUrl = tocken.GenerateQueryWebChatMsgUrl(queryMsgUrlFormat);
            QueryWebChatBaseRequestParam bp = new QueryWebChatBaseRequestParam()
            {
                DeviceID = deviceId,
                Sid = tocken.wxsid,//此参数绑定不正确
                Skey = tocken.skey,
                Uin =tocken.wxuin //  wxuin new TestClass().GetJsNewData()
            };
            Uin = tocken.wxuin;//获取验证
            //首先获取查询消息列表的参数
            string keyParamFromUrl = webChatSampleCfg["WebChatMsgSyncKeyPost"];
            string requestUrl = keyParamFromUrl.Replace("{newDate}", new TestClass().GetJsNewData()).Replace("{pass_ticket}", tocken.pass_ticket);
            QueryWebChatMsgObjectParam param = new QueryWebChatMsgObjectParam()
            {
                BaseRequest = bp,
                rr = new TestClass().DateTimeToStamp(DateTime.Now) //此处时间戳获得的数据不对
            };
            string paramJson = param.ConvertJson();            
            string SyncKey = HttpClientExt.RunPosterContainerHeaderHavaParam(requestUrl, header, paramJson, cookieContainer); //HttpClientExt.RunPost(requestUrl, paramJson);
            //getFormateSyncCheckKey  
            TecentWebChatMsgSyncKey syncKeyObj = SyncKey.ConvertObject<TecentWebChatMsgSyncKey>();
            //经常出现返回的验证状态错误 1101  采集不到消息ID
            if (syncKeyObj.BaseResponse.Ret > 0)
            {//没有待采集的消息
                paramJson.CreateLog(ELogType.ParamLog);
                SyncKey.CreateLog(ELogType.ErrorLog);
                return;
            }
            //string msg= HttpClientExt.RunPost(msgUrl, paramJson);
            SyncKey.CreateLog(ELogType.ParamLog);
            //msg.CreateLog(ELogType.DataLog);
            string url = webChatSampleCfg["QueryWebChatMsgUrlPost"];
            string msgQueryUrl = tocken.FillStringFromObject(url);//查询消息的URL
            param.SyncKey = syncKeyObj.SyncKey;
            string msgQueryPAramJson = param.ConvertJson();
            string msgResponse = HttpClientExt.RunPosterContainerHeaderHavaParam(msgQueryUrl, header, msgQueryPAramJson, cookieContainer);
            WebChatMsg msg = msgResponse.ConvertObject<WebChatMsg>();
            if (msg.AddMsgList.Count > 0)
            {
                msgResponse.CreateLog(ELogType.DataInDBLog);
                //消息入库
                List<TecentMsgDataItem> msgs = msg.AddMsgList.Select<MsgContent,TecentMsgDataItem>(s=>(TecentMsgDataItem)s).ToList();
               //hack  list<派生类> 不能直接转换为 list<父类>
                WebChatMsgService msgService = new WebChatMsgService();
                msgService.AddList(msgs);
            }
        }
        string header = @"Accept:application/json, text/plain, */*
Accept-Encoding:gzip, deflate, br
Accept-Language:zh-CN,zh;q=0.8
Connection:keep-alive
Host:wx2.qq.com
Referer:https://wx2.qq.com/?&lang=zh_CN
User-Agent:Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36";

    }
}
