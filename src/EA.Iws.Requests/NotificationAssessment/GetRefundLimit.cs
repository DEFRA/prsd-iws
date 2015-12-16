namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class GetRefundLimit : IRequest<decimal>
    {
        public Guid NotificationId { get; private set; }

        public GetRefundLimit(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
