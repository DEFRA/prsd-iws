namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Views.WasteRecovery;

    public class DisposalMethodViewModel
    {
        public Guid NotificationId { get; set; }

        [Display(Name = "DisposalMethod", ResourceType = typeof(DisposalMethodResources))]
        [Required(ErrorMessageResourceName = "DisposalMethodRequired", ErrorMessageResourceType = typeof(DisposalMethodResources))]
        public string DisposalMethod { get; set; }

        public DisposalMethodViewModel()
        {
        }

        public DisposalMethodViewModel(Guid id, string disposalMethod)
        {
            NotificationId = id;
            DisposalMethod = disposalMethod;
        }
    }
}