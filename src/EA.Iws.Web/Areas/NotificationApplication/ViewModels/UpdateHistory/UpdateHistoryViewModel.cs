namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.UpdateHistory
{
    using System;
    using System.Collections.Generic;
    using Requests.Notification;

    public class UpdateHistoryViewModel
    {
        public Guid NotificationId { get; set; }

        public IList<NotificationUpdateHistorySummaryData> UpdateHistoryItems { get; set; }
    }
}