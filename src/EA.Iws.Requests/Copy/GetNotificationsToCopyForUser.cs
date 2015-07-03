namespace EA.Iws.Requests.Copy
{
    using System.Collections.Generic;
    using Prsd.Core.Mediator;

    public class GetNotificationsToCopyForUser : IRequest<IList<NotificationApplicationCopyData>>
    {
        public GetNotificationsToCopyForUser()
        {
        }
    }
}
