namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.FinancialGuaranteeAssessment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Admin;
    using Core.FinancialGuarantee;
    using Web.ViewModels.Shared;

    public class ApproveFinancialGuaranteeViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public Guid FinancialGuaranteeId { get; set; }

        private static readonly Func<string, string> RequiredValidationMessage =
            s => string.Format("The {0} field is required", s);

        public bool IsApplicationCompleted { get; set; }

        public FinancialGuaranteeStatus Status { get; set; }

        public FinancialGuaranteeDecision? Decision { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime? DecisionRequiredDate { get; set; }

        [Display(Name = "Date decision made")]
        public OptionalDateInputViewModel DecisionMadeDate { get; set; }

        [Display(Name = "Active loads permitted")]
        public int? ActiveLoadsPermitted { get; set; }

        [MaxLength(2048)]
        [Display(Name = "Reason for refusal")]
        public string ReasonForRefusal { get; set; }

        public ApproveFinancialGuaranteeViewModel()
        {
            DecisionMadeDate = new OptionalDateInputViewModel();
        }

        public ApproveFinancialGuaranteeViewModel(FinancialGuaranteeData financialGuarantee)
        {
            DecisionRequiredDate = financialGuarantee.DecisionRequiredDate;
            Status = financialGuarantee.Status;
            IsApplicationCompleted = financialGuarantee.CompletedDate.HasValue;
            DecisionMadeDate = new OptionalDateInputViewModel(financialGuarantee.DecisionDate);
            ActiveLoadsPermitted = financialGuarantee.ActiveLoadsPermitted;
            ReasonForRefusal = financialGuarantee.RefusalReason;
            Decision = financialGuarantee.Decision;
            CompletedDate = financialGuarantee.CompletedDate;
            ReceivedDate = financialGuarantee.ReceivedDate;
            ReferenceNumber = financialGuarantee.ReferenceNumber;
            IsBlanketBond = financialGuarantee.IsBlanketBond;
        }

        public bool? IsBlanketBond { get; set; }

        [MaxLength(70)]
        public string ReferenceNumber { get; set; }

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

        private bool DecisionMadeDateIsBeforeCompletedDate()
        {
            return DecisionMadeDate.IsCompleted && DecisionMadeDate.AsDateTime() < CompletedDate;
        }

        private IEnumerable<ValidationResult> ValidateRefusal()
        {
            if (DecisionMadeDateIsBeforeCompletedDate())
            {
                yield return ValidateDecisionDate();
            }

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
            if (DecisionMadeDateIsBeforeCompletedDate())
            {
                yield return ValidateDecisionDate();
            }

            if (!DecisionMadeDate.IsCompleted)
            {
                yield return DecisionMadeDateValidation();
            }

            if (IsBlanketBond.GetValueOrDefault() && string.IsNullOrEmpty(ReferenceNumber))
            {
                yield return new ValidationResult("Please enter a reference number for the blanket bond", new[] { "ReferenceNumber" });
            }

            if (!IsBlanketBond.GetValueOrDefault() && string.IsNullOrEmpty(ReferenceNumber))
            {
                yield return new ValidationResult("Please enter a reference number for the financial guarantee", new[] { "ReferenceNumber" });
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
            if (DecisionMadeDateIsBeforeCompletedDate())
            {
                yield return ValidateDecisionDate();
            }

            if (!DecisionMadeDate.IsCompleted)
            {
                yield return DecisionMadeDateValidation();
            }
        }

        private ValidationResult DecisionMadeDateValidation()
        {
            return new ValidationResult(RequiredValidationMessage("Date decision made"), new[] { "DecisionMadeDate.Day" });
        }

        private ValidationResult ValidateDecisionDate()
        {
            return new ValidationResult(string.Format("The decision date cannot be before the completed date of {0}",
                CompletedDate.Value.ToShortDateString()),
                new[] { "DecisionMadeDate.Day" });
        }
    }
}