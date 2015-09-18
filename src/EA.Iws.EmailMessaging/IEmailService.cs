namespace EA.Iws.EmailMessaging
{
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task<bool> SendEmail(string template, string mailTo, string subject, object model);
    }
}