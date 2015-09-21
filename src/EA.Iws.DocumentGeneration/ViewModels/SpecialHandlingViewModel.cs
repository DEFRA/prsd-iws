namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.NotificationApplication;

    internal class SpecialHandlingViewModel
    {
        private string requirements = string.Empty;
        private readonly string details = string.Empty;

        public string Requirements
        {
            get { return requirements; }
            private set { requirements = value; }
        }

        public string Details
        {
            get { return details; }
        }

        public SpecialHandlingViewModel(NotificationApplication notification)
        {
            if (notification.HasSpecialHandlingRequirements.GetValueOrDefault())
            {
                Requirements = "See Annex";
                details = notification.SpecialHandlingDetails;
            }
        }

        public static SpecialHandlingViewModel GetSpecialHandlingAnnexNotice(SpecialHandlingViewModel data, int annexNumber)
        {
            data.Requirements = "See Annex " + annexNumber;
            return data;
        }
    }
}