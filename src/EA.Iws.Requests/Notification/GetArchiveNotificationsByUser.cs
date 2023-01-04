namespace EA.Iws.Requests.Notification
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanAdminUserArchiveNotifications)]
    public class GetArchiveNotificationsByUser : IRequest<UserArchiveNotifications>
    {
        public GetArchiveNotificationsByUser(int pageNumber)
        {
            PageNumber = pageNumber;
        }

        public int PageNumber { get; private set; }
    }
}