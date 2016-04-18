namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.Security;

    internal class CarrierRepository : ICarrierRepository
    {
        private readonly INotificationApplicationAuthorization authorization;
        private readonly IwsContext context;

        public CarrierRepository(IwsContext context,
            INotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public void Add(CarrierCollection carrierCollection)
        {
            context.Carriers.Add(carrierCollection);
        }

        public async Task<CarrierCollection> GetByMovementId(Guid movementId)
        {
            var notificationId =
                await context.Movements.Where(m => m.Id == movementId)
                .Select(m => m.NotificationId)
                .SingleAsync();

            return await GetByNotificationId(notificationId);
        }

        public async Task<CarrierCollection> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.Carriers.SingleAsync(c => c.NotificationId == notificationId);
        }
    }
}