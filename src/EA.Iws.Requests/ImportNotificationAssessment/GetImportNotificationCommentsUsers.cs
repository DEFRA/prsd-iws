namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotificationAssessment;
    using EA.Iws.Core.Admin;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditComments)]
    public class GetImportNotificationCommentsUsers : IRequest<ImportNotificationCommentsUsersData>
    {
        public Guid NotificationId { get; private set; }
        public NotificationShipmentsCommentsType Type { get; private set; }

        public GetImportNotificationCommentsUsers(Guid notificationId, NotificationShipmentsCommentsType type)
        {
            this.NotificationId = notificationId;
            this.Type = type;
        }
    }
}
