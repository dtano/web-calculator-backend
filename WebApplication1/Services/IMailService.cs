using WebApplication1.Contracts.Email;

namespace WebApplication1.Services
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    }
}
