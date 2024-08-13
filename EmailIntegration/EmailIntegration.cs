using Microsoft.Extensions.Options;
using Moia.Shared.Encryption;
using Moia.Shared.ViewModels;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Transactions;
using System.Xml;
using System.Xml.Xsl;

namespace EMailIntegration
{
    public class EmailIntegration
    {
        public AppSettings _AppSettings;
        private string SMTPServer { get; set; }
        private string AuthEmailFrom { get; set; }
        private string AuthDomain { get; set; }
        public string EmailFrom { get; set; }
        public int EmailPort { get; set; }
        public string EmailPassword { get; set; }
        public bool EnableSSL { get; set; }
        public string x_uqu_auth { get; set; }
        public string SenderAPIKey { get; set; }
        public string EmailPostActivate { get; set; }
        public string EmailXmlPath { get; set; }
        public string TransNumber { get; set; }

        public string NotificationXsltPath { get; set; }
        private readonly string angular_root = "";
        public EmailIntegration(IOptions<AppSettings> appSettings)
        {
            angular_root = angular_root;
            _AppSettings = new AppSettings();
            _AppSettings = appSettings.Value;
            this.SMTPServer = Encription.Decrypt( this._AppSettings.EmailSetting.SMTPServer);
            this.AuthDomain = Encription.Decrypt(this._AppSettings.EmailSetting.AuthDomain);
            this.AuthEmailFrom = Encription.Decrypt(this._AppSettings.EmailSetting.AuthEmailFrom);
            this.EmailFrom = Encription.Decrypt(this._AppSettings.EmailSetting.EmailFrom);
            this.EmailPort =this._AppSettings.EmailSetting.EmailPort;
            this.EmailPassword = Encription.Decrypt(this._AppSettings.EmailSetting.EmailPassword);
            this.EnableSSL = this._AppSettings.EmailSetting.EnableSSL;
            this.x_uqu_auth = Encription.Decrypt(this._AppSettings.EmailSetting.x_uqu_auth);
            this.SenderAPIKey = Encription.Decrypt(this._AppSettings.EmailSetting.SenderAPIKey);
            this.EmailPostActivate = Encription.Decrypt(this._AppSettings.EmailSetting.EmailPostActivate);
        }

        public void Send(string sender, string[] to, string[] cc, string[] bcc, string title, string body)
        {
            //throw new NotImplementedException();
        }

        public static string RemoveSpecialChars(string str)
        {
            // Create  a string array and add the special characters you want to remove
            //Replace('\r', ' ').Replace('\n', ' ');
            string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]", "\r", "\n" };
            //Iterate the number of times based on the String array length.
            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i]))
                {
                    str = str.Replace(chars[i], " ");
                }
            }
            return str;
        }

        public void SendNotificationEmail(string email, string subject, string Body, bool htmlEnabled, AlternateView htmlView, string CC_Email, string angularRootPath, string Attachments = "")
        {
            switch (this.EmailPostActivate)
            {
                case "1":
                    this.SendEmailDefault(email, RemoveSpecialChars(subject), Body, htmlEnabled, htmlView, CC_Email, Attachments);
                    break;
                case "2":
                    LoadImagesAndXMLForEmail(angularRootPath);
                    this.SendEmailPostUQU(RemoveSpecialChars(subject), Body, email, this.EmailXmlPath, this.TransNumber, this.NotificationXsltPath, htmlEnabled, angularRootPath);
                    break;
            }
        }

        private void SendEmailDefault(string email, string subject, string Body, bool htmlEnabled, AlternateView htmlView, string CC_Email, string Attachments = "")
        {
            try
            {
                
               SmtpClient EmailSettings = new SmtpClient();

                EmailSettings.Host = SMTPServer;
                EmailSettings.Port = EmailPort;
                EmailSettings.UseDefaultCredentials = false;

                // exchange server have to put "AuthEmailFrom" and "AuthDomain" setting value on appsettings
                if (!string.IsNullOrEmpty(this.AuthDomain))
                    EmailSettings.Credentials = new NetworkCredential(AuthEmailFrom, EmailPassword, AuthDomain);
                else if (!string.IsNullOrEmpty(this.AuthEmailFrom))
                    EmailSettings.Credentials = new NetworkCredential(AuthEmailFrom, EmailPassword);
                else
                    EmailSettings.Credentials = new NetworkCredential(EmailFrom, EmailPassword);

                EmailSettings.EnableSsl = EnableSSL;
                EmailSettings.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(EmailFrom)
                };
                // mailMessage.To.Add(email);
                mailMessage.To.Add(new MailAddress(email));
                mailMessage.IsBodyHtml = htmlEnabled;
                mailMessage.Subject = subject;

                if (!string.IsNullOrEmpty(CC_Email))
                    mailMessage.CC.Add(CC_Email);

                if (htmlEnabled)
                {
                    mailMessage.AlternateViews.Add(htmlView);
                }
                if (!String.IsNullOrEmpty(Body))
                {
                    mailMessage.Body = Body;
                }
                if (Attachments != "" && Attachments != null || Attachments != string.Empty)
                {
                    foreach (var item in Attachments.Split(","))
                    {
                        mailMessage.Attachments.Add(new System.Net.Mail.Attachment(item));
                    }

                }
                EmailSettings.Send(mailMessage);
                // notification log 
                //NotificationLog log = new NotificationLog()
                //{ Subject = mailMessage.Subject, Reciever = email, SendingDate = DateTimeOffset.Now, TransactionNumber = transaction?.TransactionNumberFormatted };
                //_DbContext.NotificationLog.Add(log);
                //_DbContext.SaveChanges();
                // notification log 
                //using (var _DbContext = new MasarContext())
                //{
                //    NotificationLog log = new NotificationLog()
                //    { Subject = mailMessage.Subject, Reciever = email, SendingDate = DateTimeOffset.Now, TransactionNumber = transaction?.TransactionNumberFormatted };
                //    _DbContext.NotificationLogs.Add(log);
                //    _DbContext.SaveChanges();
                //}
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists("C:\\testemail.txt"))
                {
                    using (StreamWriter sw = System.IO.File.AppendText("C:\\testemail.txt"))
                    {
                        sw.WriteLine("Function is = Send Email " + ex.StackTrace);
                    }
                }
            }
        }

        private void SendEmailPostUQU(string subject, string message, string to, string emailXmlPath, string transNumber, string notificationXsltPath, bool htmlEnabled, string angularRootPath)
        {
            //Uri url = new Uri("");
            try
            {
                //Insert Data in EmailXML file
                //    FillNotificationMessage(emailXmlPath, message, transNumber);

                //Load XSLT file
                XslCompiledTransform XSLTransform = new XslCompiledTransform();
                XSLTransform.Load(notificationXsltPath, new XsltSettings(), new XmlUrlResolver());

                StringBuilder emailbuilder = new StringBuilder();
                XmlTextWriter xmlwriter = new XmlTextWriter(new System.IO.StringWriter(emailbuilder));
                XSLTransform.Transform(emailXmlPath, null, xmlwriter);

                string bodyText;

                XmlDocument xemaildoc = new XmlDocument();
                xemaildoc.LoadXml(emailbuilder.ToString());

                XmlNode bodyNode = xemaildoc.SelectSingleNode("//body");

                bodyText = bodyNode.InnerXml;
                if (bodyText.Length > 0)
                {
                    bodyText = bodyText.Replace("&amp;", "&");

                    bodyText = bodyText.Replace("&lt;", "<");
                    bodyText = bodyText.Replace("&gt;", ">");
                }
                //url = EmailSettings.SMTPServer;
                string RECIEVER = to;
                string MESSAGE = message;
                string SMTPSERVER = this.SMTPServer;
                string SenderAPIKey = this.SenderAPIKey;
                string x_uqu_auth = this.x_uqu_auth;
                string MessageType = "email";
                string MessageTitle = subject;
                Uri url = new Uri(SMTPSERVER + "?SenderAPIKey=" + SenderAPIKey + "&MessageType=" + MessageType + "&MessageContent=" + MESSAGE + "&MessageTitle=" + MessageTitle + "&ReceiversDirect=" + RECIEVER + "&IsDept=1");

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["x-uqu-auth"] = x_uqu_auth;
                ServicePointManager.Expect100Continue = true;
                //     ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                if (System.IO.File.Exists("C:\\testemail.txt"))
                {
                    using (StreamWriter sw = System.IO.File.AppendText("C:\\testemail.txt"))
                    {
                        sw.WriteLine("Function is = Send Email UQU");

                        sw.WriteLine("Mes= mail is sent successfully");
                    }
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string result = "";
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadLine();
                }
              
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists("C:\\testemail.txt"))
                {
                    using (StreamWriter sw = System.IO.File.AppendText("C:\\testemail.txt"))
                    {
                        sw.WriteLine("Function is = Send Email UQU");
                        sw.WriteLine("Mes=" + ex.Message);
                        sw.WriteLine("Stack Trace=" + ex.StackTrace);
                    }
                }
            }
        }
        private void LoadImagesAndXMLForEmail(string angularRootPath)
        {
            try
            {
                this.EmailXmlPath = angularRootPath + "/assets/images/EmailImages/EmailTemplate/EmailTemplates.xml";
                this.NotificationXsltPath = angularRootPath + "/assets/images/EmailImages/EmailTemplate/EmailNotificationXSLT.xslt";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
