namespace EA.Iws.RequestHandlers.RecoveryInfo
{
    using EA.Iws.Core.Shared;
using EA.Iws.Domain.NotificationApplication;
using EA.Iws.Requests.RecoveryInfo;
using Prsd.Core.Mediator;
using System.Threading.Tasks;

    internal class GetRecoveryInfoProviderHandler : IRequestHandler<GetRecoveryInfoProvider, ProvidedBy?>
    {
        private readonly INotificationApplicationRepository repository;

        public GetRecoveryInfoProviderHandler(INotificationApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ProvidedBy?> HandleAsync(GetRecoveryInfoProvider message)
        {
            var notification = await repository.GetById(message.NotificationId);

            if (notification.RecoveryInformationProvidedByImporter.HasValue)
            {
                return notification.RecoveryInformationProvidedByImporter.Value ? ProvidedBy.Importer : ProvidedBy.Notifier;
            }

            return null;
        }
    }
}
