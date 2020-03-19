namespace EA.Iws.Api.Services
{
    public interface IAppConfiguration
    {
        string Environment { get; set; }

        string ConnectionString { get; set; }

        string SiteRoot { get; set; }

        string WebSiteRoot { get; set; }

        string SendEmail { get; set; }

        string VerificationEmailTestDomains { get; set; }

        string FeedbackEmailTo { get; set; }

        string ApiSecret { get; set; }

        string ApiClientId { get; set; }

        string ApiClientCredentialSecret { get; set; }

        string ApiClientCredentialId { get; set; }

        bool MaintenanceMode { get; set; }
    }
}