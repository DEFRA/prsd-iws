namespace EA.Iws.Virus.Api.Services
{
    using System.ComponentModel;

    public class AppConfiguration
    {
        [DefaultValue("Development")]
        public string Environment { get; set; }

        [DisplayName("DefaultConnection")]
        public string ConnectionString { get; set; }

        public string SiteRoot { get; set; }

        [DefaultValue(30000)]
        public int FileSafeTimerMilliseconds { get; set; }

        [DefaultValue("~/App_Data/uploads")]
        public string FileUploadTempPath { get; set; }
    }
}