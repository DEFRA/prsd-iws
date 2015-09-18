namespace EA.Iws.EmailMessaging.Handlers
{
    using System.Threading.Tasks;
    using Domain.Events;
    using Prsd.Core.Domain;

    internal class RegistrationRejectedHandler : IEventHandler<RegistrationRejectedEvent>
    {
        private readonly IEmailService emailService;

        public RegistrationRejectedHandler(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public async Task HandleAsync(RegistrationRejectedEvent message)
        {
            await emailService.SendEmail("InternalRegistrationRejected", message.EmailAddress, "Registration rejected", null);
        }
    }
}
