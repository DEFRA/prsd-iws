namespace EA.Iws.RequestHandlers.WasteRecovery
{
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain.NotificationApplication.WasteRecovery;
    using Prsd.Core.Domain;

    internal class ProviderChangedEventHandler : IEventHandler<ProviderChangedEvent>
    {
        private readonly IwsContext context;
        private readonly IWasteRecoveryRepository repository;

        public ProviderChangedEventHandler(IwsContext context, IWasteRecoveryRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task HandleAsync(ProviderChangedEvent @event)
        {
            if (@event.NewProvider == ProvidedBy.Importer)
            {
                var wasteRecovery = await repository.GetByNotificationId(@event.NotificationId);

                if (wasteRecovery != null)
                {
                    repository.Delete(wasteRecovery);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
