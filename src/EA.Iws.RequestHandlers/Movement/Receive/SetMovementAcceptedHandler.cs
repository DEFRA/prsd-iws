namespace EA.Iws.RequestHandlers.Movement.Receive
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Movement.Receive;

    internal class SetMovementAcceptedHandler : IRequestHandler<SetMovementAccepted, Guid>
    {
        private readonly IMovementRepository movementRepository;
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public SetMovementAcceptedHandler(IMovementRepository movementRepository, IwsContext context, IUserContext userContext)
        {
            this.movementRepository = movementRepository;
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<Guid> HandleAsync(SetMovementAccepted message)
        {
            var movement = await movementRepository.GetById(message.MovementId);

            movement.Receive(message.FileId, message.DateReceived, new Domain.ShipmentQuantity(message.Quantity, message.Units), userContext.UserId);

            await context.SaveChangesAsync();

            return message.MovementId;
        }
    }
}
