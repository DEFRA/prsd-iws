namespace EA.Iws.EmailMessaging.Handlers
{
    using System.Threading.Tasks;
    using Domain.Messaging.Internal;

    internal class RegistrationRejectedHandler : IMessageHandler<RegistrationRejectedMessage>
    {
        private readonly IEmailTemplateService emailTemplateService;
        private readonly SiteInformation siteInformation;

        public RegistrationRejectedHandler(IEmailTemplateService emailTemplateService, SiteInformation siteInformation)
        {
            this.emailTemplateService = emailTemplateService;
            this.siteInformation = siteInformation;
        }

        public async Task SendAsync(RegistrationRejectedMessage message)
        {
            var email = emailTemplateService.TemplateWithDynamicModel("InternalRegistrationRejected", null);

            var mailMessage = EmailService.GenerateMailMessageWithHtmlAndPlainTextParts("mail@mail.com",
                message.EmailAddress, "Registration rejected", email);

            await EmailService.SendMailAsync(mailMessage, siteInformation);
        }
    }
}
