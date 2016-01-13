namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Home
{
    using Core.ImportNotification.Summary;

    public class SummaryTableContainerViewModel
    {
        public ImportNotificationSummary Details { get; set; }

        public bool ShowChangeLinks { get; set; }
    }
}