namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using NotificationApplication.Shipment;

    public class MovementNumberGenerator : IMovementNumberGenerator
    {
        private readonly INextAvailableMovementNumberGenerator nextAvailableMovementNumberGenerator;
        private readonly IMovementRepository movementRepository;
        private readonly IShipmentInfoRepository shipmentRepository;

        public MovementNumberGenerator(INextAvailableMovementNumberGenerator nextAvailableMovementNumberGenerator,
            IMovementRepository movementRepository, 
            IShipmentInfoRepository shipmentRepository)
        {
            this.nextAvailableMovementNumberGenerator = nextAvailableMovementNumberGenerator;
            this.movementRepository = movementRepository;
            this.shipmentRepository = shipmentRepository;
        }

        public async Task<int> Generate(Guid notificationId)
        {
            var maxAllowedMovements = (await shipmentRepository.GetByNotificationId(notificationId)).NumberOfShipments;
            var currentMovements = (await movementRepository.GetAllMovements(notificationId)).ToArray();

            if (currentMovements.Length == maxAllowedMovements)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create a new movement number for notification {0} because there are no more available.",
                        notificationId));
            }

            return await nextAvailableMovementNumberGenerator.GetNext(notificationId);
        }
    }
}