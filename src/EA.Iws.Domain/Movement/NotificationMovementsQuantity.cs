namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Shared;
    using NotificationApplication.Shipment;

    public class NotificationMovementsQuantity
    {
        private readonly IShipmentInfoRepository shipmentRepository;
        private readonly IMovementRepository movementRepository;

        public NotificationMovementsQuantity(IMovementRepository movementRepository, IShipmentInfoRepository shipmentRepository)
        {
            this.movementRepository = movementRepository;
            this.shipmentRepository = shipmentRepository;
        }

        public async Task<ShipmentQuantity> Received(Guid notificationId)
        {
            var receivedMovements = await movementRepository.GetMovementsByStatus(notificationId, MovementStatus.Received);
            var completedMovements = await movementRepository.GetMovementsByStatus(notificationId, MovementStatus.Completed);
            var movements = receivedMovements.Union(completedMovements);

            var shipment = await shipmentRepository.GetByNotificationId(notificationId);

            if (!HasSummableMovements(movements))
            {
                return new ShipmentQuantity(0, shipment == null ? ShipmentQuantityUnits.Tonnes : shipment.Units);
            }

            var totalReceived = movements.Sum(m =>
                ShipmentQuantityUnitConverter.ConvertToTarget(
                    m.Receipt.QuantityReceived.Units,
                    shipment.Units,
                    m.Receipt.QuantityReceived.Quantity));

            return new ShipmentQuantity(totalReceived, shipment.Units);
        }

        public async Task<ShipmentQuantity> Remaining(Guid notificationId)
        {
            var shipment = await shipmentRepository.GetByNotificationId(notificationId);

            var shipmentQuantity = new ShipmentQuantity(shipment.Quantity, shipment.Units);

            return shipmentQuantity - await Received(notificationId);
        }

        private bool HasSummableMovements(IEnumerable<Movement> movements)
        {
            return movements != null && movements.Any();
        }
    }
}