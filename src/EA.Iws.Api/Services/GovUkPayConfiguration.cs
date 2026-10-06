namespace EA.Iws.Api.Services
{
    using Domain.NotificationAssessment;

    public class GovUkPayConfiguration : IGovUkPayConfiguration
    {
        private readonly AppConfiguration configuration;

        public GovUkPayConfiguration(AppConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Description
        {
            get { return configuration.GovUkPayDescription; }
        }

        public string ReturnUrlFormat
        {
            get { return configuration.GovUkPayReturnUrlFormat; }
        }
    }
}