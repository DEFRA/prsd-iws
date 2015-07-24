namespace EA.Iws.EmailMessaging
{
    public class EmailTemplate
    {
        public string HtmlText { get; private set; }

        public string PlainText { get; private set; }

        public EmailTemplate(string htmlText, string plainText)
        {
            HtmlText = htmlText;
            PlainText = plainText;
        }
    }
}