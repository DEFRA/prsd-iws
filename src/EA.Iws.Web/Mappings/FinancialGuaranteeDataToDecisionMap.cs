namespace EA.Iws.Web.Mappings
{
    using Areas.Admin.ViewModels;
    using Areas.Admin.ViewModels.FinancialGuarantee;
    using Core.Admin;
    using Prsd.Core.Mapper;

    public class FinancialGuaranteeDataToDecisionMap : IMap<FinancialGuaranteeData, FinancialGuaranteeDecisionViewModel>
    {
        public FinancialGuaranteeDecisionViewModel Map(FinancialGuaranteeData source)
        {
            return new FinancialGuaranteeDecisionViewModel
            {
                DecisionRequiredDate = source.DecisionRequiredDate,
                Status = source.Status,
                IsApplicationCompleted = source.CompletedDate.HasValue,
                ApprovedFrom = new OptionalDateInputViewModel(source.ValidFrom),
                ApprovedTo = new OptionalDateInputViewModel(source.ValidTo),
                DecisionMadeDate = new OptionalDateInputViewModel(source.DecisionDate),
                ActiveLoadsPermitted = source.ActiveLoadsPermitted,
                ReasonForRefusal = source.RefusalReason,
                Decision = source.Decision
            };
        }
    }
}