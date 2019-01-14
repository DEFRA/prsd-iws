namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.UpdateHistory
{
    using System;
    using System.Collections.Generic;
    using Requests.Notification;

    public class UpdateHistoryViewModel
    {
        public UpdateHistoryViewModel()
        {
        }
        
        public UpdateHistoryViewModel(IList<NotificationUpdateHistory> notificationUpdateHistory)
        {
            UpdateHistoryItems = new List<NotificationUpdateHistory>();
            foreach (NotificationUpdateHistory updateHistory in notificationUpdateHistory)
            {
                UpdateHistoryItems.Add(updateHistory);
            }
        }

        public Guid NotificationId { get; set; }

        public IList<NotificationUpdateHistory> UpdateHistoryItems { get; set; }
    }
}