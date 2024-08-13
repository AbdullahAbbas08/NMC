using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Net.Http.Json;
using Moia.Shared.ViewModels;
using Microsoft.Extensions.Options;
using Moia.Shared.Encryption;

namespace SmsIntegration
{
    public interface ISmsIntegrations
    {
        ReturnSMSTypes Send(string to, string[] body, string TextMessage, string templateCode = null);
    }

    public class SmsIntegrations : ISmsIntegrations
    {
        // private MainDbContext _DbContext = new MainDbContext();
        public string Account { get; set; }
        public string Password { get; set; }
        public string ServiceUrl { get; set; }
        public string SmsPostActivate { get; set; }
        public string SenderAPIKey { get; set; }
        public string x_uqu_auth { get; set; }
        public string SMS_SENDER { get; set; }
        //Dawan
        public string senderName { get; set; }
        public string LoginURl { get; set; }
        public string apiKey { get; set; }
        public string UserName { get; set; }
        public string sendMessageURL { get; set; }
        public SmsIntegrations(IOptions<AppSettings> appSettings)
        {
            this.Account = Encription.Decrypt(appSettings.Value.SMSSettings.Account);
            this.Password = Encription.Decrypt(appSettings.Value.SMSSettings.Password);
            this.ServiceUrl = Encription.Decrypt(appSettings.Value.SMSSettings.ServiceUrl);
            this.SmsPostActivate = appSettings.Value.SMSSettings.SmsPostActivate;
            this.SenderAPIKey = Encription.Decrypt(appSettings.Value.SMSSettings.SenderAPIKey);
            this.x_uqu_auth = appSettings.Value.SMSSettings.x_uqu_auth;
            this.SMS_SENDER = Encription.Decrypt(appSettings.Value.SMSSettings.SMS_SENDER);
            //Dawan
            this.senderName = appSettings.Value.SMSSettings.senderName;
            this.LoginURl = appSettings.Value.SMSSettings.LoginURl;
            this.apiKey = appSettings.Value.SMSSettings.apiKey;
            this.UserName = appSettings.Value.SMSSettings.UserName;
            this.sendMessageURL = appSettings.Value.SMSSettings.sendMessageURL;
        }

        #region Helpers
        private string fourDigits(string val)
        {
            string result = string.Empty;

            switch (val.Length)
            {
                case 1: result = "000" + val; break;
                case 2: result = "00" + val; break;
                case 3: result = "0" + val; break;
                case 4: result = val; break;
            }

            return result;
        }
        private string convertToUnicode_(char ch)
        {
            System.Text.UnicodeEncoding class1 = new System.Text.UnicodeEncoding();
            byte[] msg = class1.GetBytes(System.Convert.ToString(ch));

            return this.fourDigits(msg[1] + msg[0].ToString("X"));
        }
        #endregion

        public ReturnSMSTypes Send(string to, string[] body, string TextMessage, string templateCode = null)
        {
            try
            {
                if (File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("inside Send");
                    }
                }
                return this.SendSMSPostAffairsNew(to, body, TextMessage);
            }
            catch (Exception ex)
            {
                return new ReturnSMSTypes { success = false, message = " SmsPostActivate Must be from 1 to 8" };
            }


        }
        private ReturnSMSTypes SendSMSPostAffairsNew(string recievermobilenumber, string[] body, string message)
        {
            try
            {
                string response = "";
                string serviceUrl = this.ServiceUrl;

                if (File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("start recievermobilenumber=" + recievermobilenumber + " " + "message" + message);
                    }
                }

                string receiver = recievermobilenumber.Length > 9
                                      ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
                                      : recievermobilenumber;
                receiver = "966" + receiver;
                string userName = HttpUtility.UrlEncode(this.Account);
                string password = HttpUtility.UrlEncode(this.Password);
                string sender = HttpUtility.UrlEncode(this.SMS_SENDER); //"Hajj sender"
                string mobileNumber = HttpUtility.UrlEncode(receiver);
                string text = HttpUtility.UrlEncode(message);


                string requestUrl = serviceUrl + "?user=" + userName + "&pwd=" + password + "&senderid=" + sender + "&CountryCode=966&mobileno=" + mobileNumber + "&msgtext=" + text;

                using (WebClient client = new WebClient())
                {
                    response = client.DownloadString(requestUrl);
                }
                return new ReturnSMSTypes { success = true, message = "Sending Successfully" };
            }
            catch (Exception ex)
            {
                return new ReturnSMSTypes { success = false, message = "Sending Fail because " + ex.InnerException.ToString() };
            }
        }


        public class SendSMSRequest
        {
            public string Username { get; set; }

            public string Password { get; set; }

            public string Tagname { get; set; }

            public string RecepientNumber { get; set; }

            public string VariableList { get; set; }

            public string ReplacementList { get; set; }

            public string Message { get; set; }

            public long SendDateTime { get; set; }
        }
        public class SendSMSRequestNDF
        {
            public string userName { get; set; }

            public string numbers { get; set; }

            public string userSender { get; set; }

            public string apiKey { get; set; }

            public string msg { get; set; }

        }
        public class Message
        {
            public string senderName { get; set; }
            public string messageType { get; set; } = "text";
            public string recipients { get; set; }
            public string messageText { get; set; }

        }
        public class LoginKey
        {
            public string apiKey { get; set; }
            public string userName { get; set; }
        }
        public class LoginResponse
        {
            public int replyCode { get; set; }
            public string replyMessage { get; set; }
            public string requestId { get; set; }
            public string clientRequestId { get; set; }
            public string requestTime { get; set; }
            public Data data { get; set; }
        }
        public class Data
        {
            public string access_token { get; set; }
        }
        public class SendSMSRequestForJDA
        {
            public string number { get; set; }
            public string senderName { get; set; }
            public string sendAtOption { get; set; }
            public string messageBody { get; set; }
            public string allow_duplicate { get; set; }
        }
    }

    public class ReturnSMSTypes
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}
