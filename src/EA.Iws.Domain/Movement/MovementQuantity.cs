namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Shared;
    using NotificationApplication;

    public class MovementQuantity
    {
        private readonly ReceivedMovements getReceivedMovements;

        public MovementQuantity(ReceivedMovements receivedMovements)
        {
            this.getReceivedMovements = receivedMovements;
        }

        public decimal Received(ShipmentInfo shipmentInfo, IList<Movement> movements)
        {
            var receivedMovements = getReceivedMovements.List(movements);

            if (!HasReceivedMovements(receivedMovements))
            {
                return 0;
            }

            return receivedMovements.Sum(m => 
                ShipmentQuantityUnitConverter.ConvertToTarget(
                    m.Units.Value, 
                    shipmentInfo.Units, 
                    m.Receipt.Quantity.Value));
        }

        public decimal Remaining(ShipmentInfo shipmentInfo, IList<Movement> movements)
        {
            return shipmentInfo.Quantity - Received(shipmentInfo, movements);
        }

        private bool HasReceivedMovements(IList<Movement> receivedMovements)
        {
            return receivedMovements != null && receivedMovements.Count > 0;
        }
    }
}
