﻿namespace EA.Iws.Web.Services
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
    }
}