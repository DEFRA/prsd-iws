namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.Movement;
    using Core.Shared;
    using NotificationApplication.Shipment;

    [AutoRegister]
    public class NotificationMovementsQuantity
    {
        private readonly IShipmentInfoRepository shipmentRepository;
        private readonly IMovementRepository movementRepository;
        private readonly IMovementPartialRejectionRepository movementPartialRejectionRepository;

        public NotificationMovementsQuantity(IMovementRepository movementRepository, IShipmentInfoRepository shipmentRepository,
            IMovementPartialRejectionRepository movementPartialRejectionRepository)
        {
            this.movementRepository = movementRepository;
            this.shipmentRepository = shipmentRepository;
            this.movementPartialRejectionRepository = movementPartialRejectionRepository;
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

            var totalPartialReceived = Convert.ToDecimal(0);
            var totalPartialRejected = Convert.ToDecimal(0);

            var partialMovements = await movementRepository.GetMovementsByStatus(notificationId, MovementStatus.PartiallyRejected);
            var listOfMovementIds = partialMovements.ToArray().Select(r => r.Id);
            var listOfPartialRejectedMovements = await movementPartialRejectionRepository.GetMovementPartialRejectionsByMovementIds(listOfMovementIds);

            if (listOfPartialRejectedMovements != null && listOfPartialRejectedMovements.Count() > 0)
            {
                totalPartialReceived = listOfPartialRejectedMovements.Sum(m =>
                ShipmentQuantityUnitConverter.ConvertToTarget(
                        m.ActualUnit,
                        shipment.Units,
                        m.ActualQuantity));

                totalPartialRejected = listOfPartialRejectedMovements.Sum(m =>
                ShipmentQuantityUnitConverter.ConvertToTarget(
                        m.RejectedUnit,
                        shipment.Units,
                        m.RejectedQuantity));
            }

            totalReceived = totalReceived + (totalPartialReceived - totalPartialRejected);

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

        /// <summary>
        /// 1 ton = 1000 litres
        /// 1 ton = 1 cubic metre
        /// </summary>
        /// <param name="notificationId"></param>
        /// <param name="totalShipments"></param>
        /// <returns></returns>
        public async Task<ShipmentQuantity> AveragePerShipment(Guid notificationId, decimal totalShipments)
        {
            if (totalShipments == 0)
            {
                return new ShipmentQuantity(0, ShipmentQuantityUnits.Tonnes);
            }

            var shipment = await shipmentRepository.GetByNotificationId(notificationId);

            decimal shipmentQuantity;

            if (shipment.Units == ShipmentQuantityUnits.Kilograms)
            {
                shipmentQuantity = ShipmentQuantityUnitConverter.ConvertToTarget(
                       shipment.Units,
                       ShipmentQuantityUnits.Tonnes,
                       shipment.Quantity);
            }
            else if (shipment.Units == ShipmentQuantityUnits.Litres)
            {
                shipmentQuantity = shipment.Quantity / 1000m;
            }
            else
            {
                shipmentQuantity = shipment.Quantity;
            }

            return new ShipmentQuantity(decimal.Divide(shipmentQuantity, totalShipments), ShipmentQuantityUnits.Tonnes);
        }
    }
}