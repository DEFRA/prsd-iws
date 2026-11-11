namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.FinancialGuaranteeDecisionHistory
{
    using System.Collections.Generic;
    using Core.FinancialGuarantee;
    using Core.Notification;

    public class FinancialGuaranteeDecisionHistoryViewModel
    {
        public FinancialGuaranteeDataWithAmounts CurrentFinancialGuarantee { get; set; }

        public IEnumerable<FinancialGuaranteeData> FinancialGuaranteeHistory { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }
    }
}