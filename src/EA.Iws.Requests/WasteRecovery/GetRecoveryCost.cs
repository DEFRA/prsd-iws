namespace EA.Iws.Requests.WasteRecovery
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetRecoveryCost : IRequest<ValuePerWeightData>
    {
        public Guid NotificationId { get; private set; }

        public GetRecoveryCost(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
