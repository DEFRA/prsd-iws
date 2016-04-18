namespace EA.Iws.RequestHandlers.Movement
{
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class CancelMovementsHandler : IRequestHandler<CancelMovements, bool>
    {
        private readonly IwsContext context;
        private readonly IMovementRepository repository;

        public CancelMovementsHandler(IwsContext context, IMovementRepository repository)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(CancelMovements message)
        {
            var movementIds = message.CancelledMovements.Select(m => m.Id);

            var movements = await repository.GetMovementsByIds(message.NotificationId, movementIds);

            foreach (var movement in movements)
            {
                movement.Cancel();
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
