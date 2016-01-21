namespace EA.Iws.Requests.Annexes
{
    using System;
    using Core.Annexes.ExportNotification;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetAnnexes : IRequest<ProvidedAnnexesData>
    {
        public Guid NotificationId { get; private set; }

        public GetAnnexes(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
