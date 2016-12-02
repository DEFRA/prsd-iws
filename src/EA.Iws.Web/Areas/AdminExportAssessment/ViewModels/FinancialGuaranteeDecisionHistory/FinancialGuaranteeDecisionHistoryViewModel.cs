namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.FinancialGuaranteeDecisionHistory
{
    using System.Collections.Generic;
    using Core.FinancialGuarantee;

    public class FinancialGuaranteeDecisionHistoryViewModel
    {
        public FinancialGuaranteeData CurrentFinancialGuarantee { get; set; }

        public IEnumerable<FinancialGuaranteeData> FinancialGuaranteeHistory { get; set; }
    }
}