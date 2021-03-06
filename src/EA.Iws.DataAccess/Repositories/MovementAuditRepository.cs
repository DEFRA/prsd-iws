﻿namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.Security;

    internal class MovementAuditRepository : IMovementAuditRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public MovementAuditRepository(IwsContext context,
            INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task Add(MovementAudit audit)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(audit.NotificationId);

            context.MovementAudits.Add(audit);
        }

        public async Task<IEnumerable<MovementAudit>> GetPagedShipmentAuditsById(Guid notificationId, int pageNumber,
            int pageSize, int? shipmentNumber)
        {
            var query = context.MovementAudits
                .Where(p => p.NotificationId == notificationId);

            if (shipmentNumber.HasValue)
            {
                query = query.Where(m => m.ShipmentNumber == shipmentNumber.Value);
            }

            query = query.OrderByDescending(x => x.ShipmentNumber)
                .ThenByDescending(x => x.DateAdded)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await query.ToArrayAsync();
        }

        public async Task<int> GetTotalNumberOfShipmentAudits(Guid notificationId, int? shipmentNumber)
        {
            var query = context.MovementAudits.Where(m => m.NotificationId == notificationId);

            if (shipmentNumber.HasValue)
            {
                query = query.Where(m => m.ShipmentNumber == shipmentNumber.Value);
            }

            return await query.CountAsync();
        }      
    }
}
