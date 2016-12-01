namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.FinancialGuaranteeDecision
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Admin;
    using Prsd.Core;
    using Web.ViewModels.Shared;

    public class ApproveFinancialGuaranteeViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public Guid FinancialGuaranteeId { get; set; }

        public DateTime CompletedDate { get; set; }

        [Display(Name = "Date decision made")]
        public OptionalDateInputViewModel DecisionMadeDate { get; set; }

        [Display(Name = "Active loads permitted")]
        public int? ActiveLoadsPermitted { get; set; }
        
        public ApproveFinancialGuaranteeViewModel()
        {
            DecisionMadeDate = new OptionalDateInputViewModel();
        }

        public ApproveFinancialGuaranteeViewModel(FinancialGuaranteeData financialGuarantee)
        {
            DecisionMadeDate = new OptionalDateInputViewModel();
            CompletedDate = financialGuarantee.CompletedDate.Value;
        }

        public bool? IsBlanketBond { get; set; }

        [Display(Name = "Reference number")]
        [MaxLength(70)]
        public string ReferenceNumber { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DecisionMadeDate.IsCompleted && DecisionMadeDate.AsDateTime() < CompletedDate)
            {
                yield return new ValidationResult(string.Format("The decision date cannot be before the completed date of {0}",
                    CompletedDate.ToShortDateString()),
                    new[] { "DecisionMadeDate.Day" });
            }

            if (!DecisionMadeDate.IsCompleted)
            {
                yield return new ValidationResult("Please enter the date the decision was made", new[] { "DecisionMadeDate.Day" });
            }

            if (DecisionMadeDate.AsDateTime() > SystemTime.UtcNow)
            {
                yield return new ValidationResult("Decision date cannot be in the future", new[] { "DecisionMadeDate.Day" });
            }

            if (!ActiveLoadsPermitted.HasValue)
            {
                yield return new ValidationResult("Please enter the number of active loads permitted", new[] { "ActiveLoadsPermitted" });
            }

            if (ActiveLoadsPermitted.HasValue && ActiveLoadsPermitted.Value <= 0)
            {
                yield return new ValidationResult("The Active loads permitted must be greater than 0", new[] { "ActiveLoadsPermitted" });
            }

            if (!IsBlanketBond.HasValue)
            {
                yield return new ValidationResult("Please select whether this is a blanket bond or not", new[] { "IsBlanketBond" });
            }

            if (string.IsNullOrWhiteSpace(ReferenceNumber))
            {
                yield return new ValidationResult("Please enter the reference number", new[] { "ReferenceNumber" });
            }
        }
    }
}