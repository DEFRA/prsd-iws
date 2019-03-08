namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.FinancialGuaranteeDecision
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.FinancialGuarantee;
    using Prsd.Core;
    using Web.ViewModels.Shared;

    public class ApproveFinancialGuaranteeViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public Guid FinancialGuaranteeId { get; set; }

        public DateTime CompletedDate { get; set; }

        [Display(ResourceType = typeof(FinancialGuaranteeDecisionResources), Name = "DecisionMadeDate")]
        public OptionalDateInputViewModel DecisionMadeDate { get; set; }

        [Display(ResourceType = typeof(FinancialGuaranteeDecisionResources), Name = "CoverAmount")]
        public decimal? CoverAmount { get; set; }

        [Display(ResourceType = typeof(FinancialGuaranteeDecisionResources), Name = "ActiveLoadsPermitted")]
        public int? ActiveLoadsPermitted { get; set; }

        [Display(ResourceType = typeof(FinancialGuaranteeDecisionResources), Name = "CalculationContinued")]
        public decimal? CalculationContinued { get; set; }

        public ApproveFinancialGuaranteeViewModel()
        {
            DecisionMadeDate = new OptionalDateInputViewModel();
        }

        public ApproveFinancialGuaranteeViewModel(FinancialGuaranteeData financialGuarantee)
        {
            DecisionMadeDate = new OptionalDateInputViewModel();
            CompletedDate = financialGuarantee.CompletedDate.Value;
        }

        [Display(ResourceType = typeof(FinancialGuaranteeDecisionResources), Name = "IsBlanketBond")]
        public bool? IsBlanketBond { get; set; }

        [Display(ResourceType = typeof(FinancialGuaranteeDecisionResources), Name = "ReferenceNumber")]
        [MaxLength(70)]
        public string ReferenceNumber { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DecisionMadeDate.IsCompleted && DecisionMadeDate.AsDateTime() < CompletedDate)
            {
                yield return new ValidationResult(string.Format(FinancialGuaranteeDecisionResources.DecisionMadeDateNotBeforeCompleteDate,
                    CompletedDate.ToShortDateString()),
                    new[] { "DecisionMadeDate.Day" });
            }

            if (!DecisionMadeDate.IsCompleted)
            {
                yield return new ValidationResult(FinancialGuaranteeDecisionResources.DecisionMadeDateRequired, new[] { "DecisionMadeDate.Day" });
            }

            if (DecisionMadeDate.AsDateTime() > SystemTime.UtcNow)
            {
                yield return new ValidationResult(FinancialGuaranteeDecisionResources.DecisionMadeDateNotInFuture, new[] { "DecisionMadeDate.Day" });
            }

            if (!ActiveLoadsPermitted.HasValue)
            {
                yield return new ValidationResult(FinancialGuaranteeDecisionResources.ActiveLoadsPermittedRequired, new[] { "ActiveLoadsPermitted" });
            }

            if (ActiveLoadsPermitted.HasValue && ActiveLoadsPermitted.Value <= 0)
            {
                yield return new ValidationResult(FinancialGuaranteeDecisionResources.ActiveLoadsPermittedGreaterThanZero, new[] { "ActiveLoadsPermitted" });
            }

            if (!IsBlanketBond.HasValue)
            {
                yield return new ValidationResult(FinancialGuaranteeDecisionResources.IsBlanketBondRequired, new[] { "IsBlanketBond" });
            }

            if (string.IsNullOrWhiteSpace(ReferenceNumber))
            {
                yield return new ValidationResult(FinancialGuaranteeDecisionResources.ReferenceNumberRequired, new[] { "ReferenceNumber" });
            }

            if (!CoverAmount.HasValue)
            {
                yield return new ValidationResult(FinancialGuaranteeDecisionResources.CoverAmountRequired, new[] { "CoverAmount" });
            }

            if (!CalculationContinued.HasValue)
            {
                yield return new ValidationResult(FinancialGuaranteeDecisionResources.CalculationContinuedRequired, new[] { "CalculationContinued" });
            }
        }
    }
}