namespace EA.Iws.Requests.Notification
{
    using Prsd.Core.Mediator;

    //TODO JR what auth do we need?
    public class GetArchiveNotificationsByUser : IRequest<UserArchiveNotifications>
    {
        public GetArchiveNotificationsByUser(int pageNumber)
        {
            PageNumber = pageNumber;
        }

        public int PageNumber { get; private set; }
    }
}