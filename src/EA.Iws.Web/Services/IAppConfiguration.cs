namespace EA.Iws.Web.Services
{
    public interface IAppConfiguration
    {
        string GoogleAnalyticsAccountId { get; set; }

        string SiteRoot { get; set; }

        string SendEmail { get; set; }

        string MailFrom { get; set; }

        string ApiUrl { get; set; }

        string ApiSecret { get; set; }

        string ApiClientId { get; set; }

        string ApiClientCredentialSecret { get; set; }

        string ApiClientCredentialId { get; set; }

        string FileUploadTempPath { get; set; }

        int FileSafeTimerMilliseconds { get; set; }

        string DonePageUrl { get; set; }

        bool MaintenanceMode { get; set; }

        string ClamAvHost { get; set; }

        int ClamAvPort { get; set; }

        bool UseLocalScan { get; set; }

        string ScanUrl { get; set; }

        string AvCertPath { get; set; }
    }
}