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
        
        public UpdateHistoryViewModel(NotificationAuditTable data)
        {
            UpdateHistoryItems = new List<NotificationAuditForDisplay>();
            foreach (NotificationAuditForDisplay updateHistory in data.TableData)
            {
                UpdateHistoryItems.Add(updateHistory);
            }

            PageSize = data.PageSize;
            PageNumber = data.PageNumber;
            NumberOfNotificationAudits = data.NumberOfNotificationAudits;
        }

        public Guid NotificationId { get; set; }

        public List<NotificationAuditForDisplay> UpdateHistoryItems { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
        
        public int NumberOfNotificationAudits { get; set; }
    }
}