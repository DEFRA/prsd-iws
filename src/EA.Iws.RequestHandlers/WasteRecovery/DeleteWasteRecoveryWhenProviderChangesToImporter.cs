namespace EA.Iws.RequestHandlers.WasteRecovery
{
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain.NotificationApplication.WasteRecovery;
    using Prsd.Core.Domain;

    internal class DeleteWasteRecoveryWhenProviderChangesToImporter : IEventHandler<ProviderChangedEvent>
    {
        private readonly IwsContext context;
        private readonly IWasteRecoveryRepository recoveryRepository;
        private readonly IWasteDisposalRepository disposalRepository;

        public DeleteWasteRecoveryWhenProviderChangesToImporter(IwsContext context, 
            IWasteRecoveryRepository recoveryRepository,
            IWasteDisposalRepository disposalRepository)
        {
            this.context = context;
            this.recoveryRepository = recoveryRepository;
            this.disposalRepository = disposalRepository;
        }

        public async Task HandleAsync(ProviderChangedEvent @event)
        {
            if (@event.NewProvider == ProvidedBy.Importer)
            {
                var wasteRecovery = await recoveryRepository.GetByNotificationId(@event.NotificationId);
                var wasteDisposal = await disposalRepository.GetByNotificationId(@event.NotificationId);

                if (wasteRecovery != null)
                {
                    recoveryRepository.Delete(wasteRecovery);
                }

                if (wasteDisposal != null)
                {
                    disposalRepository.Delete(wasteDisposal);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
