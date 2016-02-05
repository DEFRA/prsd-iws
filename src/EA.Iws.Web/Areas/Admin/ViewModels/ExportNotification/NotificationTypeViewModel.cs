namespace EA.Iws.Web.Areas.Admin.ViewModels.ExportNotification
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

        public string CompetentAuthorityContactInfo
        {
            get
            {
                switch (CompetentAuthority)
                {
                    case UKCompetentAuthority.England:
                        return "Environment Agency - Tel: 01925 542265";
                    case UKCompetentAuthority.Scotland:
                        return "Scottish Environment Protection Agency - Tel: 01786 457700";
                    case UKCompetentAuthority.NorthernIreland:
                        return "Northern Ireland Environment Agency - Tel: 028 9056 9742";
                    case UKCompetentAuthority.Wales:
                        return "Natural Resources Wales - Tel: 03000 65 3000";
                    default:
                        return string.Empty;
                }
            }
        }

        public List<string> NotificationTypes { get; set; }
    }
}