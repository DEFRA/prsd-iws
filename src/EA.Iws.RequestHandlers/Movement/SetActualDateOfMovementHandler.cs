namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class SetActualDateOfMovementHandler : IRequestHandler<SetActualDateOfMovement, Guid>
    {
        private readonly IwsContext context;
        private readonly INotificationMovementService movementService;

        public SetActualDateOfMovementHandler(IwsContext context, INotificationMovementService movementService)
        {
            this.context = context;
            this.movementService = movementService;
        }

        public async Task<Guid> HandleAsync(SetActualDateOfMovement command)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == command.MovementId);

            movement.UpdateDate(command.Date);

            await context.SaveChangesAsync();

            return command.MovementId;
        }
    }
}
