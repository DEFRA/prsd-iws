namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.FinancialGuaranteeAssessment
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core;
    using Web.ViewModels.Shared;

    public class NewFinancialGuaranteeViewModel : IValidatableObject
    {
        [Required]
        [DisplayName("Guarantee received")]
        public OptionalDateInputViewModel ReceivedDate { get; set; }

        public NewFinancialGuaranteeViewModel()
        {
            ReceivedDate = new OptionalDateInputViewModel(allowPastDates: true);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ReceivedDate.IsCompleted)
            {
                yield return new ValidationResult("Received date is required", new[] { "ReceivedDate.Day" });
            }

            if (ReceivedDate.AsDateTime() > SystemTime.UtcNow)
            {
                yield return new ValidationResult("Received date cannot be in the future", new[] { "ReceivedDate.Day" });
            }
        }
    }
}