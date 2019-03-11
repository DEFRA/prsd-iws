namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.Admin;
    using EA.Iws.Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditInternalComments)]
    public class GetNotificationCommentsUsers : IRequest<NotificationCommentsUsersData>
    {
        public Guid NotificationId { get; private set; }
        public NotificationShipmentsCommentsType Type { get; private set; }

        public GetNotificationCommentsUsers(Guid notificationId, NotificationShipmentsCommentsType type)
        {
            this.NotificationId = notificationId;
            this.Type = type;
        }
    }
}
