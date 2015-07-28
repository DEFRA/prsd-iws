namespace EA.Iws.Domain.FinancialGuarantee
{
    using Core.FinancialGuarantee;
    using Prsd.Core.Domain;

    public class FinancialGuaranteeStatusChangeEvent : IEvent
    {
        public FinancialGuarantee FinancialGuarantee { get; private set; }
        public FinancialGuaranteeStatus TargetStatus { get; private set; }

        public FinancialGuaranteeStatusChangeEvent(FinancialGuarantee financialGuarantee, FinancialGuaranteeStatus targetStatus)
        {
            FinancialGuarantee = financialGuarantee;
            TargetStatus = targetStatus;
        }
    }
}