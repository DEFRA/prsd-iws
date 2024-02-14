namespace EA.Iws.DataAccess.Repositories
{
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.Domain.Security;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    internal class NotificationAdditionalChargeRepository : INotificationAdditionalChargeRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public NotificationAdditionalChargeRepository(IwsContext context, INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task<IEnumerable<AdditionalCharge>> GetPagedNotificationAdditionalChargesById(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);

            return await context.AdditionalCharges.Where(p => p.NotificationId == notificationId)
                                                       .OrderByDescending(x => x.ChargeDate)
                                                       .ToListAsync();
        }
    }
}
