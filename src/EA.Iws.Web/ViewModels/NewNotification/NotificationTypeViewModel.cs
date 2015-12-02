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

        public CompetentAuthority CompetentAuthority { get; set; }

        public string CompetentAuthorityContactInfo
        {
            get
            {
                switch (CompetentAuthority)
                {
                    case CompetentAuthority.England:
                        return "Environment Agency - Tel: 01925 542265";
                    case CompetentAuthority.Scotland:
                        return "Scottish Environment Protection Agency - Tel: 01786 457700";
                    case CompetentAuthority.NorthernIreland:
                        return "Northern Ireland Environment Agency - Tel: 028 9056 9742";
                    case CompetentAuthority.Wales:
                        return "Natural Resources Wales - Tel: 03000 65 3000";
                    default:
                        return string.Empty;
                }
            }
        }

        public List<string> NotificationTypes { get; set; }
    }
}