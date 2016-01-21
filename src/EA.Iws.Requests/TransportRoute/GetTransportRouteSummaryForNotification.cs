namespace EA.Iws.Requests.TransportRoute
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.TransportRoute;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetTransportRouteSummaryForNotification : IRequest<TransportRouteData>
    {
        public Guid NotificationId { get; private set; }

        public GetTransportRouteSummaryForNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
