namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;
    using Prsd.Core.Domain;

    public class ImportFinancialGuaranteeStatusChange : Entity
    {
        public FinancialGuaranteeStatus Source { get; private set; }

        public FinancialGuaranteeStatus Destination { get; private set; }

        public Guid UserId { get; private set; }

        public DateTimeOffset Date { get; private set; }

        public ImportFinancialGuaranteeStatusChange()
        {
        }

        public ImportFinancialGuaranteeStatusChange(FinancialGuaranteeStatus source, 
            FinancialGuaranteeStatus destination, 
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