namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.UpdateHistory
{
    using System;
    using System.Collections.Generic;
    using Core.Notification.Audit;

    public class UpdateHistoryViewModel
    {
        public UpdateHistoryViewModel()
        {
        }
        
        public UpdateHistoryViewModel(IEnumerable<NotificationAuditForDisplay> notificationUpdateHistory)
        {
            UpdateHistoryItems = new List<NotificationAuditForDisplay>();
            foreach (NotificationAuditForDisplay updateHistory in notificationUpdateHistory)
            {
                UpdateHistoryItems.Add(updateHistory);
            }
        }

        public Guid NotificationId { get; set; }

        public List<NotificationAuditForDisplay> UpdateHistoryItems { get; set; }
    }
}