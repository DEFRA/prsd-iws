namespace EA.Iws.Web.Services
{
    using System.ComponentModel;

    public class AppConfiguration : IAppConfiguration
    {
        [DefaultValue("Development")]
        public string Environment { get; set; }

        public string GoogleAnalyticsAccountId { get; set; }

        public string SiteRoot { get; set; }

        [DefaultValue("true")]
        public string SendEmail { get; set; }

        public string MailFrom { get; set; }

        public string ApiUrl { get; set; }

        public string ApiSecret { get; set; }

        public string ApiClientId { get; set; }

        public string ApiClientCredentialSecret { get; set; }

        public string ApiClientCredentialId { get; set; }

        public double ApiTimeoutInSeconds { get; set; }

        [DefaultValue("~/App_Data/uploads")]
        public string FileUploadTempPath { get; set; }

        [DefaultValue(1000)]
        public int FileSafeTimerMilliseconds { get; set; }

        public string DonePageUrl { get; set; }

        [DefaultValue(false)]
        public bool MaintenanceMode { get; set; }

        [DefaultValue(null)]
        public string ClamAvHost { get; set; }

        [DefaultValue(null)]
        public int ClamAvPort { get; set; }

        [DefaultValue(true)]
        public bool UseLocalScan { get; set; }

        [DefaultValue("")]
        public string ScanUrl { get; set; }

        [DefaultValue("")]
        public string AvCertPath { get; set; }

        public bool ProxyEnabled { get; set; }

        public bool ByPassProxyOnLocal { get; set; }

        public string ProxyWebAddress { get; set; }

        public bool ProxyUseDefaultCredentials { get; set; }

        public string CompaniesHouseReferencePath { get; set; }

        public string CompaniesHouseBaseUrl { get; set; }

        public string OAuthTokenEndpoint { get; set; }

        public string OAuthTokenClientId { get; set; }

        public string OAuthTokenClientSecret { get; set; }

        public string CompaniesHouseScope { get; set; }

        [DefaultValue(15)]
        public int SessionTimeoutInMinutes { get; set; }

        [DefaultValue(5)]
        public int SessionTimeoutWarningInMinutes { get; set; }
    }
}