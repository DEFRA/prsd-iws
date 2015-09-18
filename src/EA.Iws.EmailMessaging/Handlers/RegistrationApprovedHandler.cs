namespace EA.Iws.EmailMessaging.Handlers
{
    using System.Threading.Tasks;
    using Domain.Events;
    using Prsd.Core.Domain;

    internal class RegistrationApprovedHandler : IEventHandler<RegistrationApprovedEvent>
    {
        private readonly IEmailService emailService;
        private readonly SiteInformation siteInformation;

        public RegistrationApprovedHandler(IEmailService emailService, SiteInformation siteInformation)
        {
            this.emailService = emailService;
            this.siteInformation = siteInformation;
        }

        public async Task HandleAsync(RegistrationApprovedEvent message)
        {
            var model = new { SignInLink = string.Format("{0}/Account/Login", siteInformation.WebUrl) };

            await emailService.SendEmail("InternalRegistrationApproved", message.EmailAddress, "Registration approved", model);
        }
    }
}
