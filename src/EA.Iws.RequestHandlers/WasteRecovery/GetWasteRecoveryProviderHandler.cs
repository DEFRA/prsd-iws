namespace EA.Iws.RequestHandlers.WasteRecovery
{
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.WasteRecovery;

    internal class GetWasteRecoveryProviderHandler : IRequestHandler<GetWasteRecoveryProvider, ProvidedBy?>
    {
        private readonly INotificationApplicationRepository repository;

        public GetWasteRecoveryProviderHandler(INotificationApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ProvidedBy?> HandleAsync(GetWasteRecoveryProvider message)
        {
            var notification = await repository.GetById(message.NotificationId);

            if (notification.WasteRecoveryInformationProvidedByImporter.HasValue)
            {
                return notification.WasteRecoveryInformationProvidedByImporter.Value ? ProvidedBy.Importer : ProvidedBy.Notifier;
            }

            return null;
        }
    }
}
