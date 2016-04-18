namespace EA.Iws.Requests.StateOfExport
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.StateOfExport;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetStateOfExportWithTransportRouteDataByNotificationId : IRequest<StateOfExportWithTransportRouteData>
    {
        public Guid Id { get; private set; }

        public GetStateOfExportWithTransportRouteDataByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}
