namespace EA.Iws.DataAccess.Repositories.Imports
{
    using EA.Iws.Domain.ImportNotification;
    using EA.Iws.Domain.Security;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    internal class ImportNotificationAdditionalChargeRepository : IImportNotificationAdditionalChargeRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization importNotificationApplicationAuthorization;

        public ImportNotificationAdditionalChargeRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization importNotificationApplicationAuthorization)
        {
            this.context = context;
            this.importNotificationApplicationAuthorization = importNotificationApplicationAuthorization;
        }

        public async Task<IEnumerable<AdditionalCharge>> GetImportNotificationAdditionalChargesById(Guid notificationId)
        {
            await importNotificationApplicationAuthorization.EnsureAccessAsync(notificationId);

            return await context.AdditionalCharges.Where(p => p.NotificationId == notificationId)
                                                       .OrderByDescending(x => x.ChargeDate)
                                                       .ToListAsync();
        }

        public async Task AddImportNotificationAdditionalCharge(AdditionalCharge additionalCharge)
        {
            await importNotificationApplicationAuthorization.EnsureAccessAsync(additionalCharge.NotificationId);

            context.AdditionalCharges.Add(additionalCharge);

            await context.SaveChangesAsync();
        }
    }
}
