namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Notification;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Shared;

    public class CreateNotificationApplication : IRequest<Guid>
    {
        public NotificationType NotificationType { get; set; }

        public CompetentAuthority CompetentAuthority { get; set; }
    }
}