namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetMovementUnitsByMovementId : IRequest<ShipmentQuantityUnits>
    {
        public Guid Id { get; private set; }

        public GetMovementUnitsByMovementId(Guid id)
        {
            Id = id;
        }
    }
}
