namespace EA.Iws.Web.Mappings
{
    using System;
    using Areas.Admin.ViewModels.FinancialGuarantee;
    using Core.Admin;
    using Prsd.Core.Mapper;
    using Requests.Admin.FinancialGuarantee;

    public class FinancialGuaranteeDecisionViewModelMap : IMapWithParameter<FinancialGuaranteeDecisionViewModel, Guid, FinancialGuaranteeDecisionRequest>
    {
        public FinancialGuaranteeDecisionRequest Map(FinancialGuaranteeDecisionViewModel source, Guid id)
        {
            switch (source.Decision)
            {
                case FinancialGuaranteeDecision.Approved:
                    return new ApproveFinancialGuarantee(id, 
                        source.DecisionMadeDate.AsDateTime().Value, 
                        source.ApprovedFrom.AsDateTime().Value,
                        source.ApprovedTo.AsDateTime().Value,
                        source.ActiveLoadsPermitted.Value);
                case FinancialGuaranteeDecision.Refused:
                    return new RefuseFinancialGuarantee(id, 
                        source.DecisionMadeDate.AsDateTime().Value, 
                        source.ReasonForRefusal);
                case FinancialGuaranteeDecision.Released:
                    return new ReleaseFinancialGuarantee(id, 
                        source.DecisionMadeDate.AsDateTime().Value);
            }

            return null;
        }
    }
}