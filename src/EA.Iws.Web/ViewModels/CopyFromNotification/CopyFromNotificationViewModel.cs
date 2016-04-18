namespace EA.Iws.Web.ViewModels.CopyFromNotification
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Requests.Copy;

    public class CopyFromNotificationViewModel
    {
        [Required(ErrorMessage = "Select a notification to copy from")]
        public Guid? SelectedNotification { get; set; }

        public IList<NotificationApplicationCopyData> Notifications { get; set; }

        public CopyFromNotificationViewModel()
        {
            Notifications = new List<NotificationApplicationCopyData>();
        }
    }
}