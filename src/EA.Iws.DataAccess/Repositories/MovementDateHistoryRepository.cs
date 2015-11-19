namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.Security;

    internal class MovementDateHistoryRepository : IMovementDateHistoryRepository
    {
        private readonly INotificationApplicationAuthorization authorization;
        private readonly IwsContext context;

        public MovementDateHistoryRepository(IwsContext context,
            INotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<IEnumerable<MovementDateHistory>> GetByMovementId(Guid movementId)
        {
            var resultQuery = from movement
                         in context.Movements
                         where movement.Id == movementId
                         from movementDateHistories
                         in context.MovementDateHistories
                            .Where(mdh => mdh.MovementId == movementId)
                            .DefaultIfEmpty()
                         select new
                         {
                             Movement = movement,
                             MovementDateHistories = movementDateHistories
                         };

            var result = await resultQuery.ToArrayAsync();

            var notificationId = result.Select(r => r.Movement).Single().NotificationId;

            await authorization.EnsureAccessAsync(notificationId);

            return result.Select(r => r.MovementDateHistories);
        }
    }
}