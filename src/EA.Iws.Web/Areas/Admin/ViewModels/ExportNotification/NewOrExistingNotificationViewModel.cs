namespace EA.Iws.Web.Areas.Admin.ViewModels.ExportNotification
{
    using System.ComponentModel.DataAnnotations;
    using Core.Notification;
    using Core.Shared;

    public class NewOrExistingNotificationViewModel
    {
        public UKCompetentAuthority CompetentAuthority { get; set; }

        public NotificationType NotificationType { get; set; }

        [Required(ErrorMessage = "Please answer this question")]
        public bool? GenerateNew { get; set; }
    }
}