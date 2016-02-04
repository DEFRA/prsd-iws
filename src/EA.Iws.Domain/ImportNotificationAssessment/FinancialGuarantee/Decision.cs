namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System;

    public class DecisionData
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime Date { get; private set; }

        public DecisionData(Guid importNotificationId, DateTime date)
        {
            ImportNotificationId = importNotificationId;
            Date = date;
        }
    }
}
