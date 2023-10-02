namespace EA.Iws.Requests.WasteComponentType
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.WasteComponentType;
    using EA.Iws.Requests.Authorization;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Collections.Generic;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetWasteComponentInfoForNotification : IRequest<Guid>
    {
        public SetWasteComponentInfoForNotification(List<WasteComponentType> wasteComponentTypes, Guid notificationId)
        {
            WasteComponentTypes = wasteComponentTypes;

            NotificationId = notificationId;
        }

        public List<WasteComponentType> WasteComponentTypes { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}
