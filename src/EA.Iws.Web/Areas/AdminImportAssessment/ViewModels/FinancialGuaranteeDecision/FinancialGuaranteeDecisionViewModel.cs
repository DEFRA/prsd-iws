namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.FinancialGuaranteeDecision
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.FinancialGuarantee;
    using Core.ImportNotificationAssessment.FinancialGuarantee;
    using Infrastructure.Validation;
    using Prsd.Core.Helpers;
    using Prsd.Core.Validation;
    using Web.ViewModels.Shared;

    public class FinancialGuaranteeDecisionViewModel
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
                    new KeyValuePair<string, FinancialGuaranteeDecision>(EnumHelper.GetShortName(d), d)),
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

        [Display(Name = "ReferenceNumber", ResourceType = typeof(FinancialGuaranteeDecisionViewModelResources))]
        [RequiredIf("Decision",
            FinancialGuaranteeDecision.Approved,
            ErrorMessageResourceType = typeof(FinancialGuaranteeDecisionViewModelResources),
            ErrorMessageResourceName = "ReferenceNumberRequired")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "RefusalReason", ResourceType = typeof(FinancialGuaranteeDecisionViewModelResources))]
        [RequiredIf("Decision",
            FinancialGuaranteeDecision.Refused,
            ErrorMessageResourceType = typeof(FinancialGuaranteeDecisionViewModelResources),
            ErrorMessageResourceName = "RefusalReasonRequired")]
        public string RefusalReason { get; set; }

        public FinancialGuaranteeDecisionViewModel()
        {
            DecisionDate = new OptionalDateInputViewModel(true);
        }

        public FinancialGuaranteeDecisionViewModel(AvailableDecisionsData data) : this()
        {
            IsReceived = data.IsReceived;
            IsCompleted = data.IsCompleted;
            AvailableDecisions = data.Decisions;
            Status = data.Status;
        }
    }
}