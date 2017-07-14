namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Domain.Security;

    internal class ImportMovementTableDataRepository : IImportMovementTableDataRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportMovementTableDataRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<IEnumerable<MovementTableData>> GetById(Guid importNotificationId, int pageNumber, int pageSize)
        {
            await authorization.EnsureAccessAsync(importNotificationId);

            var movements = context.ImportMovements
                .Where(movement => movement.NotificationId == importNotificationId)
                .OrderByDescending(m => m.Number)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var result = GetMovementTableData(movements);

            return result.OrderByDescending(m => m.Number);
        }

        public async Task<IEnumerable<MovementTableData>> GetById(Guid importNotificationId)
        {
            await authorization.EnsureAccessAsync(importNotificationId);

            var movements = context.ImportMovements
                .Where(movement => movement.NotificationId == importNotificationId)
                .OrderBy(m => m.Number);

            var result = GetMovementTableData(movements);

            return result.OrderBy(m => m.Number);
        }

        private List<MovementTableData> GetMovementTableData(IEnumerable<ImportMovement> movements)
        {
            var movementData = movements
                .GroupJoin(context.ImportMovementReceipts, movement => movement.Id,
                    mr => mr.MovementId, (movement, movementReceipt) => new { movement, movementReceipt })
                .SelectMany(x => x.movementReceipt.DefaultIfEmpty(), (x, movementReceipt) => new { x.movement, movementReceipt })
                .GroupJoin(context.ImportMovementRejections, x => x.movement.Id,
                    movementRejection => movementRejection.MovementId,
                    (x, movementRejection) => new { x.movement, x.movementReceipt, movementRejection })
                .SelectMany(x => x.movementRejection.DefaultIfEmpty(),
                    (x, movementRejection) => new { x.movement, x.movementReceipt, movementRejection })
                .GroupJoin(context.ImportMovementCompletedReceipts, x => x.movement.Id,
                    movementOperationReceipt => movementOperationReceipt.MovementId,
                    (x, movementOperationReceipt) => new { x.movement, x.movementReceipt, x.movementRejection, movementOperationReceipt })
                .SelectMany(x => x.movementOperationReceipt.DefaultIfEmpty(), (x, movementOperationReceipt) => new
                {
                    Movement = x.movement,
                    MovementReceipt = x.movementReceipt,
                    MovementRejection = x.movementRejection,
                    MovementOperationReceipt = movementOperationReceipt
                });

            var result = new List<MovementTableData>();

            foreach (var movement in movementData)
            {
                result.Add(MovementTableData.Load(
                    movement.Movement,
                    movement.MovementReceipt,
                    movement.MovementRejection,
                    movement.MovementOperationReceipt));
            }

            return result;
        }

        public async Task<int> GetTotalNumberOfShipments(Guid importNotificationId)
        {
            return await context.ImportMovements.CountAsync(m => m.NotificationId == importNotificationId);
        }
    }
}
