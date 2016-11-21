namespace EA.Iws.Web.Mappings
{
    using Areas.AdminExportAssessment.ViewModels.FinancialGuarantee;
    using Core.Admin;
    using Prsd.Core.Mapper;
    using ViewModels.Shared;

    public class FinancialGuaranteeDataToDecisionMap : IMap<FinancialGuaranteeData, FinancialGuaranteeDecisionViewModel>
    {
        public FinancialGuaranteeDecisionViewModel Map(FinancialGuaranteeData source)
        {
            return new FinancialGuaranteeDecisionViewModel
            {
                DecisionRequiredDate = source.DecisionRequiredDate,
                Status = source.Status,
                IsApplicationCompleted = source.CompletedDate.HasValue,
                DecisionMadeDate = new OptionalDateInputViewModel(source.DecisionDate),
                ActiveLoadsPermitted = source.ActiveLoadsPermitted,
                ReasonForRefusal = source.RefusalReason,
                Decision = source.Decision,
                CompletedDate = source.CompletedDate,
                ReceivedDate = source.ReceivedDate,
                ReferenceNumber = source.ReferenceNumber,
                IsBlanketBond = source.IsBlanketBond
            };
        }
    }
}