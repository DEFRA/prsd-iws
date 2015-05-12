namespace EA.Iws.Cqrs.Notification
{
    using System.Collections.Generic;
    using Domain.Notification;
    using Prsd.Core.Mediator;

    public class GetNotificationsByUser : IRequest<IList<NotificationApplicationSummaryData>>
    {
    }
}