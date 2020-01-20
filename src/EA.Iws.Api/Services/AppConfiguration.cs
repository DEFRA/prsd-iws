﻿namespace EA.Iws.Api.Services
{
    using System.ComponentModel;

    public class AppConfiguration : IAppConfiguration
    {
        [DefaultValue("Development")]
        public string Environment { get; set; }

        [DisplayName("DefaultConnection")]
        public string ConnectionString { get; set; }

        public string SiteRoot { get; set; }

        public string WebSiteRoot { get; set; }

        public string SendEmail { get; set; }

        public string VerificationEmailTestDomains { get; set; }

        public string FeedbackEmailTo { get; set; }

        public string ApiSecret { get; set; }

        public string ApiClientId { get; set; }

        public string ApiClientCredentialSecret { get; set; }

        public string ApiClientCredentialId { get; set; }

        public bool MaintenanceMode { get; set; }
    }
}