namespace EA.Iws.Api.Services
{
    using System.ComponentModel;

    public class AppConfiguration
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
    }
}