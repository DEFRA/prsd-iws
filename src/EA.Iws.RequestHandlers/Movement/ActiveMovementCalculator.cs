namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using DataAccess;
    using Domain.Movement;
    using EA.Prsd.Core;
    using System.Threading.Tasks;

    internal class ActiveMovementCalculator : IActiveMovementCalculator
    {
        private readonly IwsContext context;
        private readonly ActiveMovement activeMovement;

        public ActiveMovementCalculator(IwsContext context, ActiveMovement activeMovement)
        {
            this.context = context;
            this.activeMovement = activeMovement;
        }

        public async Task<int> TotalActiveMovementsAsync(Guid notificationId)
        {
            var movements = await context.Movements.Where(m =>
                    m.NotificationApplicationId == notificationId)
                .ToListAsync();

            return movements.Where(m => activeMovement.IsActive(m)).Count();
        }
    }
}
