namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetMovementIdByNumberHandler : IRequestHandler<GetMovementIdByNumber, Guid>
    {
        private readonly IMovementRepository movementRepository;

        public GetMovementIdByNumberHandler(IMovementRepository movementRepository)
        {
            this.movementRepository = movementRepository;
        }

        public async Task<Guid> HandleAsync(GetMovementIdByNumber message)
        {
            var movement = await movementRepository.GetByNumberOrDefault(message.Number, message.NotificationId);

            if (movement == null)
            {
                throw new InvalidOperationException("No movement for notification " + message.NotificationId + " exists with number " + message.Number);
            }

            return movement.Id;
        }
    }
}
