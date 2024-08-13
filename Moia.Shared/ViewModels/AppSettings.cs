namespace Moia.Shared.ViewModels
{
    public class AppSettings
    {
        public EmailSetting EmailSetting { get; set; }
        public SMSSettings SMSSettings { get; set; }
    }

    public class EmailSetting
    {
        public string SMTPServer { get; set; }
        public string AuthEmailFrom { get; set; }
        public string AuthDomain { get; set; }
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
        public string SendEmailDelegationWithURL { get; set; }
    }

    public class SMSSettings
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string ServiceUrl { get; set; }
        public string SmsPostActivate { get; set; }
        public string SenderAPIKey { get; set; }
        public string x_uqu_auth { get; set; }
        public string SMS_SENDER { get; set; }
        public string senderName { get; set; }
        public string apiKey { get; set; }
        public string sendMessageURL { get; set; }

        public string LoginURl { get; set; }
        public string UserName { get; set; }

    }


}
