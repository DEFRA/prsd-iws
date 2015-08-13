namespace EA.Iws.Web.Areas.NotificationAssessment.ViewModels.FinancialGuarantee
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Web.ViewModels.Shared;

    public class FinancialGuaranteeDatesViewModel : IValidatableObject
    {
        [Required]
        [DisplayName("Guarantee received")]
        public OptionalDateInputViewModel Received { get; set; }

        [Required]
        [DisplayName("Guarantee complete")]
        public OptionalDateInputViewModel Completed { get; set; }

        [DisplayName("Decision required by")]
        public DateTime? DecisionRequired { get; set; }

        public string Status { get; set; }

        public bool IsRequiredEntryComplete { get; set; }

        public FinancialGuaranteeDatesViewModel()
        {
            Received = new OptionalDateInputViewModel();
            Completed = new OptionalDateInputViewModel();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Received.IsCompleted && !Completed.IsCompleted)
            {
                yield return new ValidationResult("Received date is required", new[] { "Received.Day" });
            }

            if (Received.IsCompleted && Completed.IsCompleted && Received.AsDateTime() > Completed.AsDateTime())
            {
                yield return new ValidationResult("Received date must be before completed date", new[] { "Received.Day", "Completed.Day" });
            }
        }
    }
}