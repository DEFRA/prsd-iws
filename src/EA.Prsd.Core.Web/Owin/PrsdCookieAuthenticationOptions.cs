namespace EA.Prsd.Core.Web.Owin
{
    using Microsoft.Owin;

    public class PrsdCookieAuthenticationOptions
    {
        public PrsdCookieAuthenticationOptions(string authenticationType, string apiUrl, string apiSecret)
        {
            AuthenticationType = authenticationType;
            ApiUrl = apiUrl;
            ApiSecret = apiSecret;
        }

        public string AuthenticationType { get; private set; }

        public string ApiUrl { get; private set; }

        public string ApiSecret { get; private set; }

        public PathString LoginPath { get; set; }
    }
}