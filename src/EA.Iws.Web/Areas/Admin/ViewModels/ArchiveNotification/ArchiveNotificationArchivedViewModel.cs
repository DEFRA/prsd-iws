namespace EA.Iws.Web.Areas.Admin.ViewModels.ArchiveNotification
{
    using EA.Iws.Requests.Notification;
    using System.Collections.Generic;

    public class ArchiveNotificationArchivedViewModel
    {
        public ArchiveNotificationArchivedViewModel()
        {
        }

        public IList<NotificationArchiveSummaryData> ArchivedNotifications { get; set; }

        public int SuccessCount { get; set; }

        public int FailuredCount { get; set; }
    }
}