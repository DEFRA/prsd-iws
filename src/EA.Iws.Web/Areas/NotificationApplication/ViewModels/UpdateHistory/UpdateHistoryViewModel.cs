namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.UpdateHistory
{
    using System;
    using System.Collections.Generic;
    using Requests.Notification;

    public class UpdateHistoryViewModel
    {
        public UpdateHistoryViewModel()
        {
            //COULLM: This shouldn't be set here. Remove when other constructor is in use
            UpdateHistoryItems = new List<NotificationUpdateHistorySummaryData>();

            UpdateHistoryItems.Add(new NotificationUpdateHistorySummaryData
            {
                Date = new DateTime(),
                Id = new Guid(),
                InformationChange = "Sample record 1",
                Name = "MCoull",
                Time = new DateTime(),
                TypeOfChange = "Added"
            });
            UpdateHistoryItems.Add(new NotificationUpdateHistorySummaryData
            {
                Date = new DateTime(),
                Id = new Guid(),
                InformationChange = "Sample record 2",
                Name = "MCoull",
                Time = new DateTime(),
                TypeOfChange = "Updated"
            });
            UpdateHistoryItems.Add(new NotificationUpdateHistorySummaryData
            {
                Date = new DateTime(),
                Id = new Guid(),
                InformationChange = "Sample record 3",
                Name = "MCoull",
                Time = new DateTime(),
                TypeOfChange = "Deleted"
            });
        }

        //COULLM: This should not accept UserNotifications
        public UpdateHistoryViewModel(UserNotifications userNotifications)
        {
            //COULLM: Relevant properties should be set here.
            /*
            NumberOfNotifications = userNotifications.NumberOfNotifications;
            PageNumber = userNotifications.PageNumber;
            PageSize = userNotifications.PageSize;
            Notifications = userNotifications.Notifications;
            */

            //COULLM: This should be set to a property contained in the method input variable
            UpdateHistoryItems = new List<NotificationUpdateHistorySummaryData>();
        }

        public Guid NotificationId { get; set; }

        public IList<NotificationUpdateHistorySummaryData> UpdateHistoryItems { get; set; }
    }
}