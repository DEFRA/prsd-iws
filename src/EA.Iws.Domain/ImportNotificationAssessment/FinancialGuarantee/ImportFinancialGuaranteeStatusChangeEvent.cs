namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using Core.FinancialGuarantee;
    using Prsd.Core.Domain;

    public class ImportFinancialGuaranteeStatusChangeEvent : IEvent
    {
        public FinancialGuaranteeStatus Source { get; private set; }

        public ImportFinancialGuarantee Guarantee { get; private set; }

        public FinancialGuaranteeStatus Destination { get; set; }

        public ImportFinancialGuaranteeStatusChangeEvent(ImportFinancialGuarantee importFinancialGuarantee, 
            FinancialGuaranteeStatus source, 
            FinancialGuaranteeStatus destination)
        {
            Source = source;
            Guarantee = importFinancialGuarantee;
            Destination = destination;
        }
    }
}