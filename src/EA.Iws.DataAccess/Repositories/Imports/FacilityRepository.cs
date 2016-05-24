namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Domain.Security;

    internal class FacilityRepository : IFacilityRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public FacilityRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<FacilityCollection> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.Facilities.SingleAsync(f => f.ImportNotificationId == notificationId);
        }

        public void Add(FacilityCollection facilityCollection)
        {
            context.Facilities.Add(facilityCollection);
        }
    }
}