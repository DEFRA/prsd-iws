namespace EA.Iws.Cqrs.Notification
{
    using System;
    using System.Collections.Generic;
    using Core.Cqrs;
    using Domain.Notification;

    public class GetNotificationsByUser : IQuery<IList<NotificationApplication>>
    {
        public Guid UserId { get; private set; }

        public GetNotificationsByUser(Guid userId)
        {
            this.UserId = userId;
        }
    }
}
