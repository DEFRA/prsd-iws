namespace EA.Iws.RequestHandlers.Movement
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class CancelMovementsHandler : IRequestHandler<CancelMovements, bool>
    {
        private readonly IwsContext context;
        private readonly IMovementRepository repository;
        private readonly IMovementAuditRepository movementAuditRepository;

        public CancelMovementsHandler(IwsContext context, IMovementRepository repository,
            IMovementAuditRepository movementAuditRepository)
        {
            this.repository = repository;
            this.context = context;
            this.movementAuditRepository = movementAuditRepository;
        }

        public async Task<bool> HandleAsync(CancelMovements message)
        {
            var movementIds = message.CancelledMovements.Select(m => m.Id);

            var movements = await repository.GetMovementsByIds(message.NotificationId, movementIds);

            foreach (var movement in movements)
            {
                movement.Cancel();

                await movementAuditRepository.Add(movement, MovementAuditType.Cancelled);
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
