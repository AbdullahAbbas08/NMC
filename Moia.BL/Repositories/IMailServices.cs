using EMailIntegration;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace Moia.BL.Repositories
{
    public interface IMailServices 
    {
        void Send(string sender, string[] to, string[] cc, string[] bcc, string title, string body);
        void SendNotificationEmail(string email, string mailSubject, string Body, bool htmlEnabled, AlternateView htmlView, string CC_Email, string angularRootPath, string Attachments = "");
    }

    public class MailServices : IMailServices
    {
        private EmailIntegration EmailIntegration { get; set; }

        public MailServices(IOptions<AppSettings> appSettings)
        {
            this.EmailIntegration = new EmailIntegration(appSettings);
        }

        public void Send(string sender, string[] to, string[] cc, string[] bcc, string title, string body)
        {
            this.EmailIntegration.Send(sender, to, cc, bcc, title, body);
        }

        public void SendNotificationEmail(string email, string subject, string Body, bool htmlEnabled, AlternateView htmlView, string CC_Email,string angularRootPath, string Attachments)
        {
            this.EmailIntegration.SendNotificationEmail(email, subject, Body, htmlEnabled, htmlView, CC_Email, angularRootPath, Attachments);
        }

    }

}
