namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Home
{
    using Core.ImportNotification.Summary;

    public class SummaryTableContainerViewModel
    {
        public InProgressImportNotificationSummary Details { get; set; }

        public bool ShowChangeLinks { get; set; }
    }
}