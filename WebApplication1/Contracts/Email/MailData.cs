namespace WebApplication1.Contracts.Email
{
    public class MailData
    {
        public string ReceiverEmail { get; set; }
        public string ReceiverName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
}
