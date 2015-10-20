namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class GetMovementUnitsByMovementId : IRequest<ShipmentQuantityUnits>
    {
        public Guid Id { get; private set; }

        public GetMovementUnitsByMovementId(Guid id)
        {
            Id = id;
        }
    }
}
