namespace EA.Iws.RequestHandlers.Admin
{
    using System.Threading.Tasks;
    using EmailMessaging;
    using Prsd.Core.Mediator;
    using Requests.Admin;

    internal class SendTestEmailHandler : IRequestHandler<SendTestEmail, bool>
    {
        private readonly IEmailService emailService;

        public SendTestEmailHandler(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public async Task<bool> HandleAsync(SendTestEmail message)
        {
            return await emailService.SendEmail("Test", message.EmailTo, "Test message from IWS", new { });
        }
    }
}