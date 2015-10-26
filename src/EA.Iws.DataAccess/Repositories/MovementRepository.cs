namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Domain.Security;

    internal class MovementRepository : IMovementRepository
    {
        private readonly INotificationApplicationAuthorization notificationAuthorization;
        private readonly IwsContext context;

        public MovementRepository(IwsContext context, INotificationApplicationAuthorization notificationAuthorization)
        {
            this.context = context;
            this.notificationAuthorization = notificationAuthorization;
        }

        public async Task<IEnumerable<Movement>> GetMovementsByStatus(Guid notificationId, MovementStatus status)
        {
            await notificationAuthorization.EnsureAccessAsync(notificationId);

            return await context.Movements
                .Where(m =>
                    m.NotificationId == notificationId
                    && m.Status == status)
                .ToArrayAsync();
        }

        public async Task<Movement> GetById(Guid movementId)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == movementId);

            await notificationAuthorization.EnsureAccessAsync(movement.NotificationId);

            return movement;
        }

        public async Task<IEnumerable<Movement>> GetAllMovements(Guid notificationId)
        {
            await notificationAuthorization.EnsureAccessAsync(notificationId);

            return await context.Movements
                .Where(m => m.NotificationId == notificationId)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Movement>> GetMovementsByIds(Guid notificationId, IEnumerable<Guid> movementIds)
        {
            await notificationAuthorization.EnsureAccessAsync(notificationId);

            var movements = await context.Movements
                .Where(m => 
                    m.NotificationId == notificationId
                    && movementIds.Contains(m.Id))
                .ToArrayAsync();

            return movements;
        }
    }
}