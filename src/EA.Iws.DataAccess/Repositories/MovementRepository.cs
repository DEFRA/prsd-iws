namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Movement;

    internal class MovementRepository : IMovementRepository
    {
        private readonly IwsContext context;

        public MovementRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Movement>> GetSubmittedMovements(Guid notificationId)
        {
            // TODO: filter on submitted status
            return (await context.Movements
                .Where(m => m.NotificationId == notificationId)
                .ToArrayAsync())
                .Where(m => !m.IsReceived);
        }
    }
}