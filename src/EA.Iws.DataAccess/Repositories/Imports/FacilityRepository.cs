namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;

    internal class FacilityRepository : IFacilityRepository
    {
        private readonly ImportNotificationContext context;

        public FacilityRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<FacilityCollection> GetByNotificationId(Guid notificationId)
        {
            return await context.Facilities.SingleAsync(f => f.ImportNotificationId == notificationId);
        }

        public void Add(FacilityCollection facilityCollection)
        {
            context.Facilities.Add(facilityCollection);
        }
    }
}