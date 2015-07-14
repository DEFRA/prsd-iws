namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.Notification;

    internal class SpecialHandlingViewModel
    {
        public string Requirements { get; private set; }

        public string Details { get; private set; }

        public SpecialHandlingViewModel(NotificationApplication notification)
        {
            if (notification.HasSpecialHandlingRequirements.GetValueOrDefault())
            {
                Requirements = "See annex";
                Details = notification.SpecialHandlingDetails;
            }
        }

        public static SpecialHandlingViewModel GetSpecialHandlingAnnexNotice(SpecialHandlingViewModel data, int annexNumber)
        {
            data.Requirements = "See annex " + annexNumber;
            return data;
        }
    }
}