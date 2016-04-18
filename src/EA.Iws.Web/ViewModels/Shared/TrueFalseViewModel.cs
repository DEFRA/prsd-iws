namespace EA.Iws.Web.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;
    using Areas.NotificationApplication.Views.Facility;

    public class TrueFalseViewModel
    {
        [Required(ErrorMessageResourceName = "AnswerRequired", ErrorMessageResourceType = typeof(RecoveryPreconsentResources))]
        public bool? Value { get; set; }
    }
}