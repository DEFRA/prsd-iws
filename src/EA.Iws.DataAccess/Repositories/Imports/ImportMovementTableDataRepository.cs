namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportMovement;

    internal class ImportMovementTableDataRepository : IImportMovementTableDataRepository
    {
        private readonly ImportNotificationContext context;

        public ImportMovementTableDataRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<MovementTableData>> GetById(Guid importNotificationId)
        {
            var movements = await context.ImportMovements.Where(m => m.NotificationId == importNotificationId).ToArrayAsync();

            var result = new List<MovementTableData>();

            foreach (var m in movements)
            {
                result.Add(await GetByMovementId(m.Id));
            }

            return result;
        }

        private async Task<MovementTableData> GetByMovementId(Guid movementId)
        {
            var query =
                from movement in context.ImportMovements
                where movement.Id == movementId
                from movementReceipt
                    in context.ImportMovementReceipts.Where(mr => mr.MovementId == movementId).DefaultIfEmpty()
                from movementRejection
                    in context.ImportMovementRejections.Where(mre => mre.MovementId == movementId).DefaultIfEmpty()
                from movementOperationReceipt
                    in context.ImportMovementCompletedReceipts.Where(mcr => mcr.MovementId == movementId).DefaultIfEmpty()
                select new
                {
                    Movement = movement,
                    MovementReceipt = movementReceipt,
                    MovementRejection = movementRejection,
                    MovementOperationReceipt = movementOperationReceipt
                };

            var data = await query.SingleAsync();

            return MovementTableData.Load(
                data.Movement,
                data.MovementReceipt,
                data.MovementRejection,
                data.MovementOperationReceipt);
        }
    }
}
