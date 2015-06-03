namespace EA.Iws.Web.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Requests.Notification;
    using Requests.Shared;

    public class InitialQuestionsViewModel
    {
        public InitialQuestionsViewModel()
        {
            NotificationTypes = Enum.GetNames(typeof(NotificationType)).ToList();
        }

        [Required(ErrorMessage = "Please select a notification type")]
        public NotificationType SelectedNotificationType { get; set; }

        [Required(ErrorMessage = "Please select your competent authority")]
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