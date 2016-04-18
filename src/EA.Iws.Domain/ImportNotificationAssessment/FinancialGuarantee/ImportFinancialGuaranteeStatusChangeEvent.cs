namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using Core.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Domain;

    public class ImportFinancialGuaranteeStatusChangeEvent : IEvent
    {
        public ImportFinancialGuaranteeStatus Source { get; private set; }

        public ImportFinancialGuarantee Guarantee { get; private set; }

        public ImportFinancialGuaranteeStatus Destination { get; set; }

        public ImportFinancialGuaranteeStatusChangeEvent(ImportFinancialGuarantee importFinancialGuarantee,
            ImportFinancialGuaranteeStatus source,
            ImportFinancialGuaranteeStatus destination)
        {
            Source = source;
            Guarantee = importFinancialGuarantee;
            Destination = destination;
        }
    }
}