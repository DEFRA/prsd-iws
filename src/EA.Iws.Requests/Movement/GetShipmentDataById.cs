namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.MovementDocument;
    using Prsd.Core.Mediator;

    public class GetShipmentDataById : IRequest<ShipmentData>
    {
        public Guid ShipmentId { get; private set; }

        public GetShipmentDataById(Guid shipmentId)
        {
            ShipmentId = shipmentId;
        }
    }
}
