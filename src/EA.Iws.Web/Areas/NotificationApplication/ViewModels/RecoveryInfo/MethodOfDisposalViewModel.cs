namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.RecoveryInfo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Requests.Notification;

    public class MethodOfDisposalViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public decimal PercentageRecoverable { get; set; }

        [Required]
        [Display(Name = "Planned method of disposal of the non-recoverable waste after recovery")]
        public string MethodOfDisposal { get; set; }

        public MethodOfDisposalViewModel()
        {
        }

        public MethodOfDisposalViewModel(Guid id, decimal percentageRecoverable)
        {
            NotificationId = id;
            PercentageRecoverable = percentageRecoverable;
        }

        public SetRecoveryPercentageData ToRequest()
        {
            return new SetRecoveryPercentageData(NotificationId, false, MethodOfDisposal, PercentageRecoverable);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PercentageRecoverable >= 100M || PercentageRecoverable < 0M)
            {
                yield return new ValidationResult("The percentage (%) of recoverable material must be from 0 to 99.99");
            }
        }
    }
}