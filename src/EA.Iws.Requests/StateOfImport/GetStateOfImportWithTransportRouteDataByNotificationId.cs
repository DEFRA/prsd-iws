namespace EA.Iws.Requests.StateOfImport
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetStateOfImportWithTransportRouteDataByNotificationId : IRequest<StateOfImportWithTransportRouteData>
    {
        public Guid Id { get; private set; }

        public GetStateOfImportWithTransportRouteDataByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}
