namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.FinancialGuaranteeAssessment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core;
    using Web.ViewModels.Shared;

    public class CompleteFinancialGuaranteeViewModel : IValidatableObject
    {
        public Guid FinancialGuaranteeId { get; set; }

        [Required]
        [DisplayName("Guarantee completed")]
        public OptionalDateInputViewModel CompleteDate { get; set; }

        public DateTime ReceivedDate { get; set; }

        public CompleteFinancialGuaranteeViewModel()
        {
            CompleteDate = new OptionalDateInputViewModel(allowPastDates: true);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!CompleteDate.IsCompleted)
            {
                yield return new ValidationResult("Completed date is required", new[] { "ReceivedDate.Day" });
            }

            if (CompleteDate.AsDateTime() > SystemTime.UtcNow)
            {
                yield return new ValidationResult("Completed date cannot be in the future", new[] { "CompleteDate.Day" });
            }

            if (CompleteDate.AsDateTime() < ReceivedDate)
            {
                yield return new ValidationResult("Completed date cannot be before the received date", new[] { "CompleteDate.Day" });
            }
        }
    }
}