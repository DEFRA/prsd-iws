namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Core.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Domain;

    public class ImportFinancialGuaranteeStatusChange : Entity
    {
        public ImportFinancialGuaranteeStatus Source { get; private set; }

        public ImportFinancialGuaranteeStatus Destination { get; private set; }

        public Guid UserId { get; private set; }

        public DateTimeOffset Date { get; private set; }

        public ImportFinancialGuaranteeStatusChange()
        {
        }

        public ImportFinancialGuaranteeStatusChange(ImportFinancialGuaranteeStatus source,
            ImportFinancialGuaranteeStatus destination, 
            Guid userId, 
            DateTimeOffset date)
        {
            Source = source;
            Destination = destination;
            UserId = userId;
            Date = date;
        }
    }
}