namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.NotificationConsent;

    internal class NotificationConsentRepository : INotificationConsentRepository
    {
        private readonly IwsContext context;

        public NotificationConsentRepository(IwsContext context)
        {
            this.context = context;
        }

        public void Add(Consent consent)
        {
            context.Consents.Add(consent);
        }

        public async Task<Consent> GetByNotificationId(Guid notificationId)
        {
            return await context.Consents.SingleAsync(c => c.NotificationApplicationId == notificationId);
        }
    }
}
