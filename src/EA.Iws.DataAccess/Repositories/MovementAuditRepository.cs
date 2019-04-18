namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.Security;
    using Prsd.Core.Domain;

    internal class MovementAuditRepository : IMovementAuditRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;
        private readonly IUserContext userContext;

        public MovementAuditRepository(IwsContext context,
            INotificationApplicationAuthorization notificationApplicationAuthorization,
            IUserContext userContext)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
            this.userContext = userContext;
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
                .Where(p => p.NotificationId == notificationId)
                .OrderByDescending(x => x.DateAdded)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            if (shipmentNumber.HasValue)
            {
                query = query.Where(m => m.ShipmentNumber == shipmentNumber.Value);
            }

            return await query.ToArrayAsync();
        }

        public async Task<int> GetTotalNumberOfShipmentAudits(Guid notificationId)
        {
            return await context.MovementAudits
                .CountAsync(m => m.NotificationId == notificationId);
        }      
    }
}
