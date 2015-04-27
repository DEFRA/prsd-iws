namespace EA.Iws.Web.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class InitialQuestions
    {
        [Required(ErrorMessage = "Please select notification type")]
        public string SelectedWasteAction { get; set; }

        public string CompetentAuthority { get; set; }

        public string CompetentAuthorityContactInfo { get; set; }

        public List<string> NotificationTypeAction { get; set; }

        public InitialQuestions()
        {
            NotificationTypeAction = Enum.GetNames(typeof(WasteAction)).ToList();
        }
    }
}