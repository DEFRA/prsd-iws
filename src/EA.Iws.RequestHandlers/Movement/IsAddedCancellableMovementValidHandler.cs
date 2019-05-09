namespace EA.Iws.RequestHandlers.Movement
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class IsAddedCancellableMovementValidHandler :
        IRequestHandler<IsAddedCancellableMovementValid, AddedCancellableMovementValidation>
    {
        private readonly IMovementRepository movementRepository;

        public IsAddedCancellableMovementValidHandler(IMovementRepository movementRepository)
        {
            this.movementRepository = movementRepository;
        }

        public async Task<AddedCancellableMovementValidation> HandleAsync(IsAddedCancellableMovementValid message)
        {
            var movement = await movementRepository.GetByNumberOrDefault(message.ShipmentNumber, message.NotificationId);

            var result = new AddedCancellableMovementValidation();

            if (movement != null)
            {
                result.IsCancellableExistingShipment = movement.IsInternallyCancellable;

                result.IsNonCancellableExistingShipment = !movement.IsInternallyCancellable;

                result.Status = movement.Status;
            }

            return result;
        }
    }
}
