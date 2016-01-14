namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.Security;

    internal class FacilityRepository : IFacilityRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public FacilityRepository(IwsContext context, INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task<FacilityCollection> GetByNotificationId(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);
            return await context.Facilities.SingleAsync(f => f.NotificationId == notificationId);
        }

        public void Add(FacilityCollection facilityCollection)
        {
            context.Facilities.Add(facilityCollection);
        }

        public async Task<FacilityCollection> GetByMovementId(Guid movementId)
        {
            var notificationId =
                await context.Movements.Where(m => m.Id == movementId)
                .Select(m => m.NotificationId)
                .SingleAsync();

            return await GetByNotificationId(notificationId);
        }
    }
}