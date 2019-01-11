namespace EA.Iws.Requests.Notification
{
    using System.Collections.Generic;

    public class NotificationUpdateHistory
    {
        public NotificationUpdateHistory(IList<NotificationUpdateHistorySummaryData> notificationUpdateHistory)
        {
            UpdateHistory = notificationUpdateHistory;
        }

        public IList<NotificationUpdateHistorySummaryData> UpdateHistory { get; private set; }
    }
}
