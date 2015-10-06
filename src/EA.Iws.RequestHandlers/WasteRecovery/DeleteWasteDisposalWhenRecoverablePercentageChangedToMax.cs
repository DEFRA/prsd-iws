namespace EA.Iws.RequestHandlers.WasteRecovery
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.WasteRecovery;
    using Prsd.Core.Domain;

    internal class DeleteWasteDisposalWhenRecoverablePercentageChangedToMax : IEventHandler<PercentageChangedEvent>
    {
        private readonly IwsContext context;
        private readonly IWasteDisposalRepository repository;

        public DeleteWasteDisposalWhenRecoverablePercentageChangedToMax(IwsContext context,
            IWasteDisposalRepository repository)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task HandleAsync(PercentageChangedEvent @event)
        {
            if (@event.NewPercentage.Value == 100)
            {
                var wasteDisposal = await repository.GetByNotificationId(@event.NotificationId);

                if (wasteDisposal != null)
                {
                    repository.Delete(wasteDisposal);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
