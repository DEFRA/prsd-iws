namespace EA.Iws.EmailMessaging
{
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;
    using Autofac;

    internal class EmailService
    {
        private readonly IComponentContext context;

        public EmailService(IComponentContext context)
        {
            this.context = context;
        }

        public static MailMessage GenerateMailMessageWithHtmlAndPlainTextParts(string from, 
            string to, 
            string subject,
            EmailTemplate emailTemplate)
        {
            var mail = new MailMessage(from, to) { Subject = subject, IsBodyHtml = true };

            // Add the plain text alternative for other email clients.
            var plainText = AlternateView.CreateAlternateViewFromString(emailTemplate.PlainText, Encoding.UTF8, MediaTypeNames.Text.Plain);
            mail.AlternateViews.Add(plainText);

            // Add the HTML view.
            var htmlText = AlternateView.CreateAlternateViewFromString(emailTemplate.HtmlText, Encoding.UTF8,
                MediaTypeNames.Text.Html);
            mail.AlternateViews.Add(htmlText);

            return mail;
        }

        public static async Task SendMailAsync(MailMessage message, SiteInformation siteInformation)
        {
            var client = new SmtpClient();

            if (siteInformation.SendEmail)
            {
                await client.SendMailAsync(message);
            }
        }
    }
}
