namespace EA.Iws.Web.Areas.Admin.ViewModels.FinancialGuarantee
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.FinancialGuarantee;
    using Prsd.Core.Helpers;

    public class FinancialGuaranteeDecisionViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        private static readonly Func<string, string> RequiredValidationMessage =
            s => string.Format("The {0} field is required", s);

        public bool IsApplicationCompleted { get; set; }

        public FinancialGuaranteeStatus Status { get; set; }

        public FinancialGuaranteeDecision? Decision { get; set; }

        public SelectList PossibleDecisions { get; set; }

        public DateTime? DecisionRequiredDate { get; set; }

        [Display(Name = "Date decision made")]
        public OptionalDateInputViewModel DecisionMadeDate { get; set; }

        [Display(Name = "Approved from")]
        public OptionalDateInputViewModel ApprovedFrom { get; set; }

        [Display(Name = "Approved to")]
        public OptionalDateInputViewModel ApprovedTo { get; set; }

        [Display(Name = "Active loads permitted")]
        public int? ActiveLoadsPermitted { get; set; }

        [MaxLength(2048)]
        [Display(Name = "Reason for refusal")]
        public string ReasonForRefusal { get; set; }

        public FinancialGuaranteeDecisionViewModel()
        {
            PossibleDecisions = new SelectList(EnumHelper.GetValues(typeof(FinancialGuaranteeDecision)), "key", "value");
            DecisionMadeDate = new OptionalDateInputViewModel();
            ApprovedFrom = new OptionalDateInputViewModel();
            ApprovedTo = new OptionalDateInputViewModel();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Decision.HasValue)
            {
                return new ValidationResult[0];
            }

            switch (Decision.Value)
            {
                case FinancialGuaranteeDecision.Refused:
                    return ValidateRefusal();
                case FinancialGuaranteeDecision.Released:
                    return ValidateRelease();
                default:
                    return ValidateApproval();
            }
        }

        private IEnumerable<ValidationResult> ValidateRefusal()
        {
            if (string.IsNullOrWhiteSpace(ReasonForRefusal))
            {
                yield return new ValidationResult(RequiredValidationMessage("Reason for refusal"), new[] { "ReasonForRefusal" });
            }

            if (!DecisionMadeDate.IsCompleted)
            {
                yield return DecisionMadeDateValidation();
            }
        }

        private IEnumerable<ValidationResult> ValidateApproval()
        {
            if (!DecisionMadeDate.IsCompleted)
            {
                yield return DecisionMadeDateValidation();
            }

            if (!ApprovedFrom.IsCompleted)
            {
                yield return new ValidationResult(RequiredValidationMessage("Approved from"), new[] { "ApprovedFrom.Day" });
            }

            if (!ApprovedTo.IsCompleted)
            {
                yield return new ValidationResult(RequiredValidationMessage("Approved to"), new[] { "ApprovedTo.Day" });
            }

            if (ApprovedFrom.IsCompleted 
                && ApprovedTo.IsCompleted 
                && ApprovedFrom.AsDateTime() > ApprovedTo.AsDateTime())
            {
                yield return new ValidationResult("The Approved to date must not be before the Approved from date", new[] { "ApprovedTo.Day" });
            }

            if (!ActiveLoadsPermitted.HasValue)
            {
                yield return new ValidationResult(RequiredValidationMessage("Active loads permitted"), new[] { "ActiveLoadsPermitted" });
            }

            if (ActiveLoadsPermitted.HasValue && ActiveLoadsPermitted.Value <= 0)
            {
                yield return new ValidationResult("The Active loads permitted must be greater than 0", new[] { "ActiveLoadsPermitted" });
            }
        }

        private IEnumerable<ValidationResult> ValidateRelease()
        {
            if (!DecisionMadeDate.IsCompleted)
            {
                yield return DecisionMadeDateValidation();
            }
        }

        private ValidationResult DecisionMadeDateValidation()
        {
            return new ValidationResult(RequiredValidationMessage("Date decision made"), new[] { "DecisionMadeDate.Day" });
        }
    }
}