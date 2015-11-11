namespace EA.Iws.RequestHandlers.NotificationMovements
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;

    internal class CheckMovementNumberValidHandler : IRequestHandler<CheckMovementNumberValid, MovementNumberStatus>
    {
        private readonly IMovementRepository movementRepository;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public CheckMovementNumberValidHandler(IMovementRepository movementRepository,
            IShipmentInfoRepository shipmentInfoRepository)
        {
            this.movementRepository = movementRepository;
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<MovementNumberStatus> HandleAsync(CheckMovementNumberValid message)
        {
            var movement = await movementRepository.GetByNumberOrDefault(message.Number, message.NotificationId);

            if (movement != null)
            {
                if (movement.Status == MovementStatus.New)
                {
                    return MovementNumberStatus.Valid;
                }

                return MovementNumberStatus.NotNew;
            }

            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(message.NotificationId);

            if (shipmentInfo.NumberOfShipments < message.Number)
            {
                return MovementNumberStatus.OutOfRange;
            }

            return MovementNumberStatus.DoesNotExist;
        }
    }
}
