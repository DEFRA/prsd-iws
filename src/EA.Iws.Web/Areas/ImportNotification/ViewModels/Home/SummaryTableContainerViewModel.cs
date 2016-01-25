namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Home
{
    using Core.ImportNotification.Summary;
    using Core.Shared;

    public class SummaryTableContainerViewModel
    {
        public ImportNotificationSummary Details { get; set; }

        public bool ShowChangeLinks { get; set; }

        public bool ShowPreconsentedLinks
        {
            get { return Details.Type == NotificationType.Recovery; }
        }
    }
}