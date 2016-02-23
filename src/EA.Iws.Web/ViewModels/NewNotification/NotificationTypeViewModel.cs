namespace EA.Iws.Web.ViewModels.NewNotification
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.Notification;
    using Core.Shared;

    public class NotificationTypeViewModel
    {
        public NotificationTypeViewModel()
        {
            NotificationTypes = Enum.GetNames(typeof(NotificationType)).ToList();
        }

        [Required(ErrorMessage = "Please answer this question")]
        public NotificationType? SelectedNotificationType { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }
        
        public List<string> NotificationTypes { get; set; }
    }
}