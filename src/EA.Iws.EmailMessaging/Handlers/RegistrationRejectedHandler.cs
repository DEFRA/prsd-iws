namespace EA.Iws.EmailMessaging.Handlers
{
    using System.Threading.Tasks;
    using Autofac;
    using Domain.Events;
    using Prsd.Core.Domain;

    internal class RegistrationRejectedHandler : IEventHandler<RegistrationRejectedEvent>
    {
        private readonly IEmailTemplateService emailTemplateService;
        private readonly SiteInformation siteInformation;

        public RegistrationRejectedHandler(IEmailTemplateService emailTemplateService, SiteInformation siteInformation, IComponentContext ctxt)
        {
            this.emailTemplateService = emailTemplateService;
            this.siteInformation = siteInformation;
        }

        public async Task HandleAsync(RegistrationRejectedEvent message)
        {
            var email = emailTemplateService.TemplateWithDynamicModel("InternalRegistrationRejected", null);

            var mailMessage = EmailService.GenerateMailMessageWithHtmlAndPlainTextParts("mail@mail.com",
                message.EmailAddress, "Registration rejected", email);

            await EmailService.SendMailAsync(mailMessage, siteInformation);
        }
    }
}
