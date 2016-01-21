namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.PackagingType;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovements)]
    public class GetPackagingTypes : IRequest<PackagingData>
    {
        public Guid NotificationId { get; private set; }

        public GetPackagingTypes(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}