namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using DataAccess;
    using Domain.Movement;
    using EA.Prsd.Core;
    using System.Threading.Tasks;

    internal class ActiveMovementsService : IActiveMovementsService
    {
        private readonly IwsContext context;

        public ActiveMovementsService(IwsContext context)
        {
            this.context = context;
        }

        public async Task<int> TotalActiveMovementsAsync(Guid notificationId)
        {
            var movements = await context.Movements.Where(m =>
                    m.NotificationApplicationId == notificationId)
                .ToListAsync();

            return movements.Where(m => IsMovementActive(m)).Count();
        }

        public bool IsMovementActive(Movement movement)
        {
            return movement.Date.HasValue && movement.Date < SystemTime.UtcNow;
        }
    }
}
