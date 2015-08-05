namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.RecoveryInfo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Notification;
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

        public MethodOfDisposalViewModel(RecoveryPercentageData data)
        {
            NotificationId = data.NotificationId;
            PercentageRecoverable = data.PercentageRecoverable.GetValueOrDefault();
            MethodOfDisposal = data.MethodOfDisposal;
        }

        public SetRecoveryPercentageData ToRequest()
        {
            return new SetRecoveryPercentageData(NotificationId, false, PercentageRecoverable, MethodOfDisposal);
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