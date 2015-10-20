namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;

    internal class SetMovementRejectedHandler : IRequestHandler<SetMovementRejected, Guid>
    {
        private readonly IMovementRepository movementRepository;
        private readonly IwsContext context;

        public SetMovementRejectedHandler(IMovementRepository movementRepository, IwsContext context)
        {
            this.movementRepository = movementRepository;
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetMovementRejected message)
        {
            var movement = await movementRepository.GetById(message.MovementId);

            movement.Reject(message.FileId, message.DateReceived, message.Reason);

            await context.SaveChangesAsync();

            return message.MovementId;
        }
    }
}
