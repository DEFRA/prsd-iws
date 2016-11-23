namespace EA.Iws.RequestHandlers.Mappings.NotificationAssessment
{
    using Core.NotificationAssessment;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mapper;

    internal class FinancialGuaranteeDecisionMap :
        IMap<FinancialGuaranteeDecision, FinancialGuaranteeDecisionData>
    {
        public FinancialGuaranteeDecisionData Map(FinancialGuaranteeDecision source)
        {
            return new FinancialGuaranteeDecisionData
            {
                NotificationId = source.NotificationId,
                Date = source.Date,
                Decision = string.Format("Financial guarantee {0}", EnumHelper.GetDisplayName(source.Status).ToLowerInvariant())
            };
        }
    }
}