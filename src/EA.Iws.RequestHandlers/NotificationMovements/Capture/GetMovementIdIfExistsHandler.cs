namespace EA.Iws.RequestHandlers.NotificationMovements.Capture
{
    using System;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Capture;

    public class GetMovementIdIfExistsHandler : IRequestHandler<GetMovementIdIfExists, Guid?>
    {
        private readonly IMovementRepository movementRepository;

        public GetMovementIdIfExistsHandler(IMovementRepository movementRepository)
        {
            this.movementRepository = movementRepository;
        }

        public async Task<Guid?> HandleAsync(GetMovementIdIfExists message)
        {
            var movement = await movementRepository.GetByNumberOrDefault(message.Number, message.NotificationId);

            if (movement == null)
            {
                return null;
            }

            return movement.Id;
        }
    }
}