namespace EA.Iws.Web.Mappings.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Areas.AdminImportAssessment.ViewModels.FinancialGuaranteeDecision;
    using Core.Admin;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mapper;
    using Requests.ImportNotificationAssessment.FinancialGuarantee;

    public class FinancialGuaranteeDecisionViewModelMap : IMapWithParameter<FinancialGuaranteeDecisionViewModel, Guid, FinancialGuaranteeDecisionRequest>
    {
        public FinancialGuaranteeDecisionRequest Map(FinancialGuaranteeDecisionViewModel source, Guid parameter)
        {
            switch (source.Decision)
            {
                case FinancialGuaranteeDecision.Approved:
                    if (source.IsBlanketBond.GetValueOrDefault())
                    {
                        return new ApproveBlanketBondFinancialGuarantee(parameter, 
                            source.DecisionDate.AsDateTime().Value, 
                            source.ReferenceNumber, 
                            source.ActiveLoadsPermitted.Value,
                            source.ValidFrom.AsDateTime().Value);
                    }

                    return new ApproveFinancialGuarantee(parameter, source.DecisionDate.AsDateTime().Value,
                        source.ReferenceNumber,
                        source.ActiveLoadsPermitted.Value,
                        source.ValidFrom.AsDateTime().Value,
                        source.ValidTo.AsDateTime().Value);
                case FinancialGuaranteeDecision.Refused:
                    return new RefuseFinancialGuarantee(parameter, source.DecisionDate.AsDateTime().Value,
                        source.RefusalReason);
                case FinancialGuaranteeDecision.Released:
                    return new ReleaseFinancialGuarantee(parameter, source.DecisionDate.AsDateTime().Value);
                default:
                    break;
            }

            throw new ArgumentException("No request mapped for decision: " + EnumHelper.GetDisplayName(source.Decision));
        }
    }
}