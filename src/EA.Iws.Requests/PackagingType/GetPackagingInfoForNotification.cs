namespace EA.Iws.Requests.PackagingType
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.PackagingType;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetPackagingInfoForNotification : IRequest<PackagingData>
    {
        public GetPackagingInfoForNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}