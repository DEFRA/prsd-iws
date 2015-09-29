namespace EA.Iws.RequestHandlers.RecoveryInfo
{
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain.NotificationApplication.Recovery;
    using Prsd.Core.Domain;

    internal class ProviderChangedEventHandler : IEventHandler<ProviderChangedEvent>
    {
        private readonly IwsContext context;
        private readonly IRecoveryInfoRepository repository;

        public ProviderChangedEventHandler(IwsContext context, IRecoveryInfoRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task HandleAsync(ProviderChangedEvent @event)
        {
            if (@event.NewProvider == ProvidedBy.Importer)
            {
                var recoveryInfo = await repository.GetByNotificationId(@event.NotificationId);

                if (recoveryInfo != null)
                {
                    repository.Delete(recoveryInfo);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
