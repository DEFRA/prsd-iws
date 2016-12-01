namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.FinancialGuaranteeDecision
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Admin;
    using Prsd.Core;
    using Web.ViewModels.Shared;

    public class RefuseFinancialGuaranteeViewModel : IValidatableObject
    {
        public RefuseFinancialGuaranteeViewModel()
        {
            DecisionMadeDate = new OptionalDateInputViewModel();
        }

        public RefuseFinancialGuaranteeViewModel(FinancialGuaranteeData financialGuarantee)
        {
            DecisionMadeDate = new OptionalDateInputViewModel();
            CompletedDate = financialGuarantee.CompletedDate.Value;
        }

        public Guid NotificationId { get; set; }

        public Guid FinancialGuaranteeId { get; set; }

        public DateTime CompletedDate { get; set; }

        [Display(Name = "Date decision made")]
        public OptionalDateInputViewModel DecisionMadeDate { get; set; }

        [MaxLength(2048)]
        [Display(Name = "Reason for refusal")]
        public string ReasonForRefusal { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DecisionMadeDate.IsCompleted && DecisionMadeDate.AsDateTime() < CompletedDate)
            {
                yield return
                    new ValidationResult(string.Format("The decision date cannot be before the completed date of {0}",
                        CompletedDate.ToShortDateString()),
                        new[] { "DecisionMadeDate.Day" });
            }

            if (!DecisionMadeDate.IsCompleted)
            {
                yield return
                    new ValidationResult("Please enter the date the decision was made", new[] { "DecisionMadeDate.Day" });
            }

            if (DecisionMadeDate.AsDateTime() > SystemTime.UtcNow)
            {
                yield return new ValidationResult("Decision date cannot be in the future", new[] { "DecisionMadeDate.Day" });
            }

            if (string.IsNullOrWhiteSpace(ReasonForRefusal))
            {
                yield return new ValidationResult("Please enter the reference number", new[] { "ReasonForRefusal" });
            }
        }
    }
}