namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Domain.Security;

    internal class ImportMovementAuditRepository : IImportMovementAuditRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportMovementAuditRepository(ImportNotificationContext context,
            IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task Add(ImportMovementAudit audit)
        {
            await authorization.EnsureAccessAsync(audit.NotificationId);

            context.ImportMovementAudits.Add(audit);

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ImportMovementAudit>> GetPagedShipmentAuditsById(Guid notificationId,
            int pageNumber, int pageSize, int? shipmentNumber)
        {
            var query = context.ImportMovementAudits
                .Where(p => p.NotificationId == notificationId)
                .OrderByDescending(x => x.ShipmentNumber)
                .ThenByDescending(x => x.DateAdded)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            if (shipmentNumber.HasValue)
            {
                query = query.Where(m => m.ShipmentNumber == shipmentNumber.Value);
            }

            return await query.ToArrayAsync();
        }

        public async Task<int> GetTotalNumberOfShipmentAudits(Guid notificationId, int? shipmentNumber)
        {
            var query = context.ImportMovementAudits.Where(m => m.NotificationId == notificationId);

            if (shipmentNumber.HasValue)
            {
                query = query.Where(m => m.ShipmentNumber == shipmentNumber.Value);
            }

            return await query.CountAsync();
        }
    }
}
