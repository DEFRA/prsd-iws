namespace EA.Iws.EmailMessaging
{
    using System;

    public class SiteInformation
    {
        public string ApiUrl { get; private set; }
        public string WebUrl { get; private set; }
        public bool SendEmail { get; private set; }

        public SiteInformation(string apiUrl, string webUrl, string sendEmail)
        {
            ApiUrl = apiUrl;
            WebUrl = webUrl;

            SendEmail = !string.IsNullOrWhiteSpace(sendEmail) &&
                        sendEmail.Equals("true", StringComparison.OrdinalIgnoreCase);
        }
    }
}
