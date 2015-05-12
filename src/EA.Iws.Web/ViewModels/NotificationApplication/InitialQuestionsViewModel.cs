namespace EA.Iws.Web.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Requests.Notification;

    public class InitialQuestionsViewModel
    {
        public InitialQuestionsViewModel()
        {
            NotificationTypeAction = Enum.GetNames(typeof(WasteAction)).ToList();
        }

        [Required(ErrorMessage = "Please select a notification type")]
        public WasteAction SelectedWasteAction { get; set; }

        [Required(ErrorMessage = "Please select your competent authority")]
        public CompetentAuthority CompetentAuthority { get; set; }

        public string CompetentAuthorityContactInfo
        {
            get
            {
                switch (CompetentAuthority)
                {
                    case CompetentAuthority.England:
                        return "Environment Agency - Tel: 01253 876934";
                    case CompetentAuthority.Scotland:
                        return "Scottish Environment Protection Agency - Tel: 01253 876934";
                    case CompetentAuthority.NorthernIreland:
                        return "Northern Ireland Environment Agency - Tel: 01253 876934";
                    case CompetentAuthority.Wales:
                        return "Natural Resources Wales - Tel: 01253 876934";
                    default:
                        return string.Empty;
                }
            }
        }

        public List<string> NotificationTypeAction { get; set; }
    }
}