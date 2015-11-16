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

        public async Task<int> Generate(Guid notificationId)
        {
            List<int> newMovementNumbers = new List<int>();
            var maxAllowedMovements = (await shipmentRepository.GetByNotificationId(notificationId)).NumberOfShipments;
            var currentMovements = await movementRepository.GetAllMovements(notificationId);

            if (currentMovements.Count() == maxAllowedMovements)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create a new movement number for notification {0} because there are no more available.",
                        notificationId));
            }

            var usedNumbers = currentMovements.Select(m => m.Number);

            if (!currentMovements.Any())
            {
                return 1;
            }

            if (usedNumbers.Max() == currentMovements.Count())
            {
                return currentMovements.Count() + 1;
            }

            for (int i = 1; i < currentMovements.Count() + 1; i++)
            {
                if (!usedNumbers.Contains(i))
                {
                    return i;
                }
            }

            throw new InvalidOperationException("No available numbers (this should not have happened!)");
        }
    }
}