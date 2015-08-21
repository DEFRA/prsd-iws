namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    public class CreateMovementForNotificationByIdHandler : IRequestHandler<CreateMovementForNotificationById, Guid>
    {
        private readonly IwsContext context;
        private readonly INotificationMovementService movementService;

        public CreateMovementForNotificationByIdHandler(IwsContext context, INotificationMovementService movementService)
        {
            this.context = context;
            this.movementService = movementService;
        }

        public async Task<Guid> HandleAsync(CreateMovementForNotificationById message)
        {
            var movement = new Movement(message.Id, movementService);

            context.Movements.Add(movement);

            await context.SaveChangesAsync();

            return movement.Id;
        }
    }
}
