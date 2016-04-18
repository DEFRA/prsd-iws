namespace EA.Iws.RequestHandlers.WasteRecovery
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.WasteRecovery;

    internal class SetWasteRecoveryProviderHandler : IRequestHandler<SetWasteRecoveryProvider, bool>
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationRepository repository;

        public SetWasteRecoveryProviderHandler(INotificationApplicationRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetWasteRecoveryProvider message)
        {
            var notification = await repository.GetById(message.NotificationId);

            notification.SetWasteRecoveryInformationProvider(message.ProvidedBy);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
