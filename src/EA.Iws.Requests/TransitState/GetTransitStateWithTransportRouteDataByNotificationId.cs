namespace EA.Iws.Requests.TransitState
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetTransitStateWithTransportRouteDataByNotificationId : IRequest<TransitStateWithTransportRouteData>
    {
        public Guid Id { get; private set; }

        public Guid? TransitStateId { get; private set; }

        public GetTransitStateWithTransportRouteDataByNotificationId(Guid id, Guid? transitStateId)
        {
            Id = id;
            TransitStateId = transitStateId;
        }
    }
}
