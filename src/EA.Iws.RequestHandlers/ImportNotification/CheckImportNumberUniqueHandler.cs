namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class CheckImportNumberUniqueHandler : IRequestHandler<CheckImportNumberUnique, bool>
    {
        private readonly IImportNotificationRepository importNotificationRepository;

        public CheckImportNumberUniqueHandler(IImportNotificationRepository importNotificationRepository)
        {
            this.importNotificationRepository = importNotificationRepository;
        }

        public async Task<bool> HandleAsync(CheckImportNumberUnique message)
        {
            return !await importNotificationRepository.NotificationNumberExists(message.NotificationNumber);
        }
    }
}
