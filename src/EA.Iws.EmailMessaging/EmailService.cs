namespace EA.Iws.EmailMessaging
{
    using System.Threading.Tasks;
    using Prsd.Email;

    internal class EmailService : IEmailService
    {
        private readonly ITemplateExecutor templateExecutor;
        private readonly IMessageCreator messageCreator;
        private readonly ISender sender;

        public EmailService(ITemplateExecutor templateExecutor, IMessageCreator messageCreator, ISender sender)
        {
            this.templateExecutor = templateExecutor;
            this.messageCreator = messageCreator;
            this.sender = sender;
        }

        public async Task<bool> SendEmail(string template, string mailTo, string subject, object model)
        {
            var content = new EmailContent
            {
                HtmlText = templateExecutor.Execute(template + ".cshtml", model),
                PlainText = templateExecutor.Execute(template + ".txt", model)
            };

            var message = messageCreator.Create(mailTo, subject, content);

            return await sender.SendAsync(message);
        }
    }
}