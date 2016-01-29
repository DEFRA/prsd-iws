namespace EA.Iws.Requests.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Prsd.Core.Mediator;

    public class GetReceivedDate : IRequest<DateTime?>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetReceivedDate(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
