namespace EA.Iws.Web.Areas.Movement.ViewModels.Reject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Web.ViewModels.Shared;

    public class RejectViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }
        
        [Display(Name = "DateName", ResourceType = typeof(RejectViewModelResources))]
        public OptionalDateInputViewModel RejectionDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(RejectViewModelResources), ErrorMessageResourceName = "RejectionReasonRequired")]
        [Display(Name = "RejectionReasonName", ResourceType = typeof(RejectViewModelResources))]
        public string RejectionReason { get; set; }

        [Display(Name = "FileName", ResourceType = typeof(RejectViewModelResources))]
        [Required(ErrorMessageResourceType = typeof(RejectViewModelResources), ErrorMessageResourceName = "FileRequired")]
        public HttpPostedFileBase File { get; set; }

        public RejectViewModel()
        {
            RejectionDate = new OptionalDateInputViewModel(true);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!RejectionDate.IsStarted)
            {
                yield return new ValidationResult(RejectViewModelResources.DateRequired, new[] { "RejectionDate.Day" });
            }
        }
    }
}