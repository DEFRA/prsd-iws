namespace EA.Iws.EmailMessaging.Handlers
{
    using System.Threading.Tasks;
    using Domain.Events;
    using Prsd.Core.Domain;

    internal class RegistrationApprovedHandler : IEventHandler<RegistrationApprovedEvent>
    {
        private readonly IEmailTemplateService emailTemplateService;
        private readonly SiteInformation siteInformation;

        public RegistrationApprovedHandler(IEmailTemplateService emailTemplateService, SiteInformation siteInformation)
        {
            this.emailTemplateService = emailTemplateService;
            this.siteInformation = siteInformation;
        }

        public async Task HandleAsync(RegistrationApprovedEvent message)
        {
            var email = emailTemplateService.TemplateWithDynamicModel("InternalRegistrationApproved",
                new { SignInLink = string.Format("{0}/Account/Login", siteInformation.WebUrl) });

            var mailMessage = EmailService.GenerateMailMessageWithHtmlAndPlainTextParts("mailfrom@mail.com", message.EmailAddress,
                "Registration approved", email);

            await EmailService.SendMailAsync(mailMessage, siteInformation);
        }
    }
}
