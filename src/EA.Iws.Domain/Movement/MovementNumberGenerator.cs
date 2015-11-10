namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using NotificationApplication.Shipment;

    public class MovementNumberGenerator
    {
        private readonly IShipmentInfoRepository shipmentRepository;
        private readonly IMovementRepository movementRepository;

        public MovementNumberGenerator(IMovementRepository movementRepository, 
            IShipmentInfoRepository shipmentRepository)
        {
            this.movementRepository = movementRepository;
            this.shipmentRepository = shipmentRepository;
        }

        public async Task<IList<int>> Generate(Guid notificationId, int newMovementsCount)
        {
            List<int> newMovementNumbers = new List<int>();
            var maxAllowedMovements = (await shipmentRepository.GetByNotificationId(notificationId)).NumberOfShipments;
            var currentMovementNumber = (await movementRepository.GetAllMovements(notificationId)).Count();

            if (currentMovementNumber + newMovementsCount > maxAllowedMovements)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create {0} new movements because this would take the number of movements above the maximum allowed of {1}",
                        newMovementsCount, maxAllowedMovements));
            }

            for (int i = currentMovementNumber; i < currentMovementNumber + newMovementsCount; i++)
            {
                newMovementNumbers.Add(i + 1);
            }

            return newMovementNumbers;
        }
    }
}