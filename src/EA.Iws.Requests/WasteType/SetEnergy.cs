namespace EA.Iws.Requests.WasteType
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetEnergy : IRequest<Guid>
    {
        public SetEnergy(string energyInformation, Guid notificationId)
        {
            EnergyInformation = energyInformation;
            NotificationId = notificationId;
        }

        public string EnergyInformation { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}