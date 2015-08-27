namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class SetActualDateOfMovementHandler : IRequestHandler<SetActualDateOfMovement, Guid>
    {
        private readonly IwsContext context;

        public SetActualDateOfMovementHandler(IwsContext context)
        {
            this.context = context;
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
