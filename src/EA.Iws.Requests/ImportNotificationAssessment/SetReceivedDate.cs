namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class SetReceivedDate : IRequest<bool>
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime ReceivedDate { get; private set; }

        public SetReceivedDate(Guid importNotificationId, DateTime receivedDate)
        {
            ImportNotificationId = importNotificationId;
            ReceivedDate = receivedDate;
        }
    }
}
