namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.FinancialGuaranteeDecision
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Helpers;

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
                    new KeyValuePair<string, FinancialGuaranteeDecision>(EnumHelper.GetDisplayName(d), d)),
                    "Value",
                    "Key");
            }
        }

        public FinancialGuaranteeDecision? Decision { get; set; }

        public FinancialGuaranteeDecisionViewModel()
        {
        }

        public FinancialGuaranteeDecisionViewModel(AvailableDecisionsData data)
        {
            IsReceived = data.IsReceived;
            IsCompleted = data.IsCompleted;
            AvailableDecisions = data.Decisions;
            Status = data.Status;
        }
    }
}