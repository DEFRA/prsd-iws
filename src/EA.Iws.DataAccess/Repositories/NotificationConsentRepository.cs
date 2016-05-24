namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.NotificationConsent;
    using Domain.Security;

    internal class NotificationConsentRepository : INotificationConsentRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization authorization;

        public NotificationConsentRepository(IwsContext context, INotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public void Add(Consent consent)
        {
            context.Consents.Add(consent);
        }

        public async Task<Consent> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.Consents.SingleAsync(c => c.NotificationApplicationId == notificationId);
        }
    }
}
