namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.FinancialGuaranteeDecision
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.ImportNotificationAssessment.FinancialGuarantee;
    using Infrastructure.Validation;
    using Prsd.Core.Helpers;
    using Prsd.Core.Validation;
    using Web.ViewModels.Shared;

    public class FinancialGuaranteeDecisionViewModel : IValidatableObject
    {
        public ImportFinancialGuaranteeStatus Status { get; set; }

        public bool IsReceived { get; set; }

        public bool IsCompleted { get; set; }

        public IList<FinancialGuaranteeDecision> AvailableDecisions { get; set; }

        public SelectList AvailableDecisionsList
        {
            get
            {
                return new SelectList(AvailableDecisions.Select(d => 
                    new KeyValuePair<string, FinancialGuaranteeDecision>(EnumHelper.GetDisplayName(d), d)),
                    "Value",
                    "Key");
            }
        }

        [Display(Name = "Decision", ResourceType = typeof(FinancialGuaranteeDecisionViewModelResources))]
        [Required(ErrorMessageResourceName = "DecisionRequired", ErrorMessageResourceType = typeof(FinancialGuaranteeDecisionViewModelResources))]
        public FinancialGuaranteeDecision? Decision { get; set; }

        [Display(Name = "DecisionDate", ResourceType = typeof(FinancialGuaranteeDecisionViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceType = typeof(FinancialGuaranteeDecisionViewModelResources), 
            ErrorMessageResourceName = "DecisionDateRequired")]
        public OptionalDateInputViewModel DecisionDate { get; set; }

        [Display(Name = "IsBlanketBond", ResourceType = typeof(FinancialGuaranteeDecisionViewModelResources))]
        [RequiredIf("Decision", 
            FinancialGuaranteeDecision.Approved, 
            ErrorMessageResourceType = typeof(FinancialGuaranteeDecisionViewModelResources), 
            ErrorMessageResourceName = "IsBlanketBondRequired")]
        public bool? IsBlanketBond { get; set; }

        [Display(Name = "ValidFrom", ResourceType = typeof(FinancialGuaranteeDecisionViewModelResources))]
        public OptionalDateInputViewModel ValidFrom { get; set; }

        [Display(Name = "ValidTo", ResourceType = typeof(FinancialGuaranteeDecisionViewModelResources))]
        public OptionalDateInputViewModel ValidTo { get; set; }

        [RequiredIf("Decision",
            FinancialGuaranteeDecision.Approved,
            ErrorMessageResourceType = typeof(FinancialGuaranteeDecisionViewModelResources),
            ErrorMessageResourceName = "ReferenceNumberRequired")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "ActiveLoads", ResourceType = typeof(FinancialGuaranteeDecisionViewModelResources))]
        [RequiredIf("Decision",
            FinancialGuaranteeDecision.Approved,
            ErrorMessageResourceType = typeof(FinancialGuaranteeDecisionViewModelResources),
            ErrorMessageResourceName = "ActiveLoadsRequired")]
        public int? ActiveLoadsPermitted { get; set; }

        [Display(Name = "RefusalReason", ResourceType = typeof(FinancialGuaranteeDecisionViewModelResources))]
        [RequiredIf("Decision",
            FinancialGuaranteeDecision.Refused,
            ErrorMessageResourceType = typeof(FinancialGuaranteeDecisionViewModelResources),
            ErrorMessageResourceName = "RefusalReasonRequired")]
        public string RefusalReason { get; set; }

        public FinancialGuaranteeDecisionViewModel()
        {
            ValidTo = new OptionalDateInputViewModel(true);
            ValidFrom = new OptionalDateInputViewModel(true);
            DecisionDate = new OptionalDateInputViewModel(true);
        }

        public FinancialGuaranteeDecisionViewModel(AvailableDecisionsData data) : this()
        {
            IsReceived = data.IsReceived;
            IsCompleted = data.IsCompleted;
            AvailableDecisions = data.Decisions;
            Status = data.Status;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Decision.HasValue)
            {
                switch (Decision)
                {
                    case FinancialGuaranteeDecision.Approved:
                        return ValidateApproval();
                    default:
                        break;
                }
            }

            return new ValidationResult[0];
        }

        private IEnumerable<ValidationResult> ValidateApproval()
        {
            if (!ValidFrom.IsCompleted)
            {
                yield return new ValidationResult(FinancialGuaranteeDecisionViewModelResources.ValidFromRequired, new[] { "ValidFrom.Day" });
            }

            if (!ValidTo.IsCompleted && !IsBlanketBond.GetValueOrDefault())
            {
                yield return new ValidationResult(FinancialGuaranteeDecisionViewModelResources.ValidToRequired, new[] { "ValidTo.Day" });
            }

            if (ValidFrom.IsCompleted && ValidTo.IsCompleted
                && ValidFrom.AsDateTime() > ValidTo.AsDateTime())
            {
                yield return new ValidationResult(FinancialGuaranteeDecisionViewModelResources.ValidToBeforeValidFrom, new[] { "ValidTo.Day" });
            }
        } 
    }
}