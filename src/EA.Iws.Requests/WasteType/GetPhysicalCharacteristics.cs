namespace EA.Iws.Requests.WasteType
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.WasteType;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetPhysicalCharacteristics : IRequest<PhysicalCharacteristicsData>
    {
        public GetPhysicalCharacteristics(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}