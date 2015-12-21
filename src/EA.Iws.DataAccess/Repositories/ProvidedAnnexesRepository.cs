namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Threading.Tasks;
    using Core.Notification;
    using Domain.NotificationApplication;

    internal class ProvidedAnnexesRepository : IProvidedAnnexesRepository
    {
        private readonly IwsContext context;

        public ProvidedAnnexesRepository(IwsContext context)
        {
            this.context = context;
        }

        public Task<ProvidedAnnexesData> GetProvidedAnnexesData(Guid notificationId)
        {
            throw new NotImplementedException();
        }
    }
}
