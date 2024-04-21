using Microsoft.Extensions.Options;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;
using WebApplication1.Contracts;
using WebApplication1.Contracts.Email;

namespace WebApplication1.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }

        public bool SendMail(MailData mailData)
        {
            var apiInstance = new TransactionalEmailsApi();

            SendSmtpEmailSender sender = new SendSmtpEmailSender(_mailSettings.SenderName, _mailSettings.SenderEmail);


            List<SendSmtpEmailTo> destinations = new List<SendSmtpEmailTo>();
            SendSmtpEmailTo receiver = new SendSmtpEmailTo(mailData.ReceiverEmail, mailData.ReceiverName);
            destinations.Add(receiver);
            
            
            string HtmlContent = null;
            string TextContent = mailData.EmailBody;

            string Subject = mailData.EmailSubject;
            try
            {
                var sendSmtpEmail = new SendSmtpEmail(sender, destinations, null, null, HtmlContent, TextContent, Subject);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);

                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to send welcome email: " + e.Message);
            }
        }
    }
}
