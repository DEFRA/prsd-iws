namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
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

        public async Task<Movement> GetById(Guid movementId)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == movementId);
            await notificationAuthorization.EnsureAccessAsync(movement.NotificationId);
            return movement;
        }

        public async Task<IEnumerable<Movement>> GetSubmittedMovements(Guid notificationId)
        {
            await notificationAuthorization.EnsureAccessAsync(notificationId);

            // TODO: filter on submitted status
            return (await context.Movements
                .Where(m => m.NotificationId == notificationId)
                .ToArrayAsync())
                .Where(m => !m.IsReceived);
        }
    }
}