namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Core.Shared;

    public class DisposalMethodViewModel
    {
        public Guid NotificationId { get; set; }

        [Required(ErrorMessage = "Please enter the method of disposal")]
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