namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class ObjectNotificationApplication : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public ObjectNotificationApplication(Guid id)
        {
            Id = id;
        }
    }
}