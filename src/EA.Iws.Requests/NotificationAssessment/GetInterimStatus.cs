namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    public class GetInterimStatus : IRequest<InterimStatus>
    {
        public GetInterimStatus(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}