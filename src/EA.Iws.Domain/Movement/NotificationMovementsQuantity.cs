namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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

        public async Task<decimal> Received(Guid notificationId)
        {
            var receivedMovements = await movementRepository.GetReceivedMovements(notificationId);
            var completedMovements = await movementRepository.GetCompletedMovements(notificationId);
            var movements = receivedMovements.Union(completedMovements);

            var shipment = await shipmentRepository.GetByNotificationId(notificationId);

            if (!HasSummableMovements(movements))
            {
                return 0;
            }

            var totalReceived = movements.Sum(m =>
                ShipmentQuantityUnitConverter.ConvertToTarget(
                    m.Units.Value,
                    shipment.Units,
                    m.Receipt.Quantity.Value));

            return totalReceived;
        }

        public async Task<decimal> Remaining(Guid notificationId)
        {
            var shipment = await shipmentRepository.GetByNotificationId(notificationId);

            return shipment.Quantity - await Received(notificationId);
        }

        private bool HasSummableMovements(IEnumerable<Movement> movements)
        {
            return movements != null && movements.Any();
        }
    }
}
