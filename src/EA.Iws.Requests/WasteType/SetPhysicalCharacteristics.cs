namespace EA.Iws.Requests.WasteType
{
    using System;
    using System.Collections.Generic;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.WasteType;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetPhysicalCharacteristics : IRequest<Guid>
    {
        public SetPhysicalCharacteristics(List<PhysicalCharacteristicType> physicalCharacteristics, Guid notificationId, string otherDescription)
        {
            PhysicalCharacteristics = physicalCharacteristics;
            NotificationId = notificationId;
            OtherDescription = otherDescription;
        }

        public List<PhysicalCharacteristicType> PhysicalCharacteristics { get; private set; }

        public Guid NotificationId { get; private set; }

        public string OtherDescription { get; private set; }
    }
}