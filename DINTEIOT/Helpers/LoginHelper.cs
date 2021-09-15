using DINTEIOT;
using DINTEIOT.Models.Account;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace LoginHelper
{
    public static class LoginHelper
    {

        static string key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";


        public static string Encrypt(string text)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateEncryptor())
                    {
                        byte[] textBytes = UTF8Encoding.UTF8.GetBytes(text);
                        byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        public static string Decrypt(string cipher)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {
                        byte[] cipherBytes = Convert.FromBase64String(cipher);
                        byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        return UTF8Encoding.UTF8.GetString(bytes);
                    }
                }
            }
        }
        public static void InsertToXmlFile(string fileName, string userid, string messagesend, string timersend, string receiverid)
        {
            try
            {


                if (File.Exists(fileName))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fileName);
                    if (doc.SelectSingleNode("root") == null)
                    {
                        XElement xml_pre = XElement.Load(fileName);
                        xml_pre.Add(new XElement("message"));
                        xml_pre.Save(fileName);
                    }
                    XElement xml = XElement.Load(fileName);
                    xml.Add(new XElement("doc",
                    new XElement("message",
                    new XAttribute("userid", userid),
                    new XAttribute("messagesend", messagesend),
                    new XAttribute("timersend", timersend),
                    new XAttribute("receiverid", receiverid))));
                    xml.Save(fileName);
                }
                else
                {
                    XDocument doc =
                    new XDocument(new XElement("root",
                    new XElement("doc",
                    new XElement("message",
                    new XAttribute("userid", userid),
                    new XAttribute("messagesend", messagesend),
                    new XAttribute("timersend", timersend),
                    new XAttribute("receiverid", receiverid)
                    )
                )));
                    doc.Save(fileName, SaveOptions.None);

                }



            }
            catch (Exception e)
            {

            }

        }

        public static string DateToStringAbout(DateTime? dateTime)
        {
            var timeabout = "";
            if (string.IsNullOrEmpty(dateTime.ToString()))
            {
                return null;
            }
            DateTime startTime = DateTime.Now;
            TimeSpan span = startTime.Subtract(dateTime.Value);
            if (span.Days != 0)
            {
                return timeabout = span.Days + " ngày trước";
            }
            else if (span.Hours != 0)
            {
                return timeabout = span.Hours + " giờ trước";
            }
            else if (span.Minutes != 0)
            {
                return timeabout = span.Minutes + " phút trước";
            }
            return timeabout;
        }
        public async static Task GetThinkBoardToken()
        {
            var token = new ThinkBoardToken();
            try
            {
                var httpClient = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                httpRequestMessage.RequestUri = new Uri(Startup.ConnectionStringsThinkBoard + "/api/auth/login");

                // Tạo StringContent
                string jsoncontent = JsonConvert.SerializeObject(new { username  = Startup.usernametb, password = Startup.passwordtb });
                var httpContent = new StringContent(jsoncontent, Encoding.UTF8, "application/json");
                httpRequestMessage.Content = httpContent;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<ThinkBoardToken>(responseContent);
                Startup.thinkportaccesstoken = token.token;
            }
            catch(Exception e)
            {

            }
        }
        public class JsonContent : StringContent
        {
            public JsonContent(object obj) :
                base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
            { }
        }
        public class ThinkBoardToken
        {
            public string token { get; set; }
        }
    }
}