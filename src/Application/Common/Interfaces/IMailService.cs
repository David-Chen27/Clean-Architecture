namespace Clean_Architecture.Application.Common.Interfaces
{
    public interface IMailService
    {
        void SendMail(string to, string subject, string body);
    }
}
