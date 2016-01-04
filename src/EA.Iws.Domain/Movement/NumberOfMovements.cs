namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using NotificationApplication.Shipment;

    public class NumberOfMovements
    {
        private readonly IMovementRepository movementRepository;
        private readonly IShipmentInfoRepository shipmentRepository;

        public NumberOfMovements(IMovementRepository movementRepository, IShipmentInfoRepository shipmentRepository)
        {
            this.movementRepository = movementRepository;
            this.shipmentRepository = shipmentRepository;
        }

        public async Task<bool> HasMaximum(Guid notificationId)
        {
            var maxNumberOfShipments = (await shipmentRepository.GetByNotificationId(notificationId)).NumberOfShipments;
            var currentNumberOfShipments = (await movementRepository.GetAllMovements(notificationId)).Count();

            return currentNumberOfShipments >= maxNumberOfShipments;
        }
    }
}