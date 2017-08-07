namespace EA.Iws.Web.Areas.ExportMovement.ViewModels.Reject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Infrastructure.Validation;
    using Web.ViewModels.Shared;

    public class RejectViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }
        
        [Display(Name = "DateName", ResourceType = typeof(RejectViewModelResources))]
        public OptionalDateInputViewModel RejectionDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(RejectViewModelResources), ErrorMessageResourceName = "RejectionReasonValidationMessage")]
        [Display(Name = "RejectionReasonTitle", ResourceType = typeof(RejectViewModelResources))]
        public string RejectionReason { get; set; }

        [Display(Name = "FileName", ResourceType = typeof(RejectViewModelResources))]
        [Required(ErrorMessageResourceType = typeof(RejectViewModelResources), ErrorMessageResourceName = "FileRequired")]
        [RestrictToAllowedUploadTypes]
        public HttpPostedFileBase File { get; set; }

        public DateTime MovementDate { get; set; }

        public RejectViewModel()
        {
            RejectionDate = new OptionalDateInputViewModel(true)
            {
                IsAutoTabEnabled = false
            };
        }

        public RejectViewModel(DateTime movementDate)
        {
            RejectionDate = new OptionalDateInputViewModel(true)
            {
                IsAutoTabEnabled = false
            };

            MovementDate = movementDate;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!RejectionDate.IsStarted)
            {
                yield return new ValidationResult(RejectViewModelResources.DateRequired, new[] { "RejectionDate.Day" });
            }

            var rejectionDate = RejectionDate.AsDateTime();
            if (rejectionDate.HasValue && rejectionDate.Value < MovementDate)
            {
                yield return new ValidationResult(RejectViewModelResources.RejectionDateNotBeforeShipmentDate, new[] { "RejectionDate.Day" });
            }
        }
    }
}