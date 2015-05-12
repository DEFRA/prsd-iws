namespace EA.Iws.Requests.Notification
{
    using System.Collections.Generic;
    using Prsd.Core.Mediator;

    public class GetNotificationsByUser : IRequest<IList<NotificationApplicationSummaryData>>
    {
    }
}