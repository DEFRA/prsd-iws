namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.FinancialGuaranteeAssessment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core;
    using Web.ViewModels.Shared;

    public class CompleteFinancialGuaranteeViewModel : IValidatableObject
    {
        public Guid FinancialGuaranteeId { get; set; }

        [Required]
        [Display(ResourceType = typeof(FinancialGuaranteeAssessmentResources), Name = "CompleteDate")]
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
                yield return new ValidationResult(FinancialGuaranteeAssessmentResources.CompleteDateRequired, new[] { "CompleteDate.Day" });
            }

            if (CompleteDate.AsDateTime() > SystemTime.UtcNow)
            {
                yield return new ValidationResult(FinancialGuaranteeAssessmentResources.CompleteDateNotInFuture, new[] { "CompleteDate.Day" });
            }

            if (CompleteDate.AsDateTime() < ReceivedDate)
            {
                yield return new ValidationResult(FinancialGuaranteeAssessmentResources.CompleteDateNotBeforeReceivedDate, new[] { "CompleteDate.Day" });
            }
        }
    }
}