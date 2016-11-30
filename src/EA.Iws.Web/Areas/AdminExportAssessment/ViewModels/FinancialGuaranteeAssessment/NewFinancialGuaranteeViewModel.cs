namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.FinancialGuaranteeAssessment
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Web.ViewModels.Shared;

    public class NewFinancialGuaranteeViewModel : IValidatableObject
    {
        [Required]
        [DisplayName("Guarantee received")]
        public OptionalDateInputViewModel ReceivedDate { get; set; }

        [Required]
        [DisplayName("Guarantee complete")]
        public OptionalDateInputViewModel CompletedDate { get; set; }

        public NewFinancialGuaranteeViewModel()
        {
            ReceivedDate = new OptionalDateInputViewModel(allowPastDates: true);
            CompletedDate = new OptionalDateInputViewModel(allowPastDates: true);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ReceivedDate.IsCompleted)
            {
                yield return new ValidationResult("Received date is required", new[] { "ReceivedDate.Day" });
            }

            if (ReceivedDate.IsCompleted && CompletedDate.IsCompleted && ReceivedDate.AsDateTime() > CompletedDate.AsDateTime())
            {
                yield return new ValidationResult("Received date must be before completed date", new[] { "ReceivedDate.Day" });
            }
        }
    }
}