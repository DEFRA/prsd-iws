namespace EA.Iws.EmailMessaging
{
    public class SiteInformation
    {
        public SiteInformation(string apiUrl, string webUrl)
        {
            ApiUrl = apiUrl;
            WebUrl = webUrl;
        }

        public string ApiUrl { get; private set; }

        public string WebUrl { get; private set; }
    }
}