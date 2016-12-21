namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Domain.Security;
    using Prsd.Core;

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

        public async Task<Movement> GetByNumberOrDefault(int movementNumber, Guid notificationId)
        {
            await notificationAuthorization.EnsureAccessAsync(notificationId);

            return await context.Movements.Where(m => m.NotificationId == notificationId
            && m.Number == movementNumber).SingleOrDefaultAsync();
        }

        public void Add(Movement movement)
        {
            context.Movements.Add(movement);
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

        public async Task<IEnumerable<Movement>> GetActiveMovements(Guid notificationId)
        {
            await notificationAuthorization.EnsureAccessAsync(notificationId);

            var currentActiveLoads = await context.Movements
                .Where(m =>
                    m.NotificationId == notificationId
                    && (m.Status == MovementStatus.Submitted
                        || m.Status == MovementStatus.Received)
                    && m.Date < SystemTime.UtcNow).ToArrayAsync();

            return currentActiveLoads;
        }

        public async Task<int> GetLatestMovementNumber(Guid notificationId)
        {
            await notificationAuthorization.EnsureAccessAsync(notificationId);

            var movement = await context.Movements.Where(m => m.NotificationId == notificationId).OrderByDescending(m => m.Date).ThenByDescending(m => m.Number).FirstOrDefaultAsync();

            return movement == null ? 0 : movement.Number;
        }

        public async Task<bool> DeleteById(Guid movementId)
        {
            var rowsAffected = await context.Database.ExecuteSqlCommandAsync(
                @"DELETE from [Notification].[MovementCarrier] WHERE MovementDetailsId in (SELECT Id FROM [Notification].[MovementDetails] WHERE MovementId = @movementId)
                  DELETE from [Notification].[MovementPackagingInfo] WHERE MovementDetailsId in (SELECT Id FROM [Notification].[MovementDetails] WHERE MovementId = @movementId)
                  DELETE from [Notification].[MovementDetails] WHERE MovementId = @movementId
                  DELETE from [Notification].[MovementDateHistory] WHERE MovementId = @movementId
                  DELETE from [Notification].[MovementOperationReceipt] WHERE MovementId = @movementId
                  DELETE from [Notification].[MovementReceipt] WHERE MovementId = @movementId
                  DELETE from [Notification].[MovementRejection] WHERE MovementId = @movementId
                  DELETE from [Notification].[MovementStatusChange] WHERE MovementId = @movementId
                  DELETE from [Notification].[Movement] WHERE Id = @movementId",
                new SqlParameter("@movementId", movementId));

            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Movement>> GetRejectedMovements(Guid notificationId)
        {
            await notificationAuthorization.EnsureAccessAsync(notificationId);

            return await context.Movements.Where(m => m.NotificationId == notificationId && (m.Status == MovementStatus.Rejected)).ToArrayAsync();
        }
    }
}