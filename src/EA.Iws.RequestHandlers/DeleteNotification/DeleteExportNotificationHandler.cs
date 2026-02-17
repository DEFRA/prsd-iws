namespace EA.Iws.RequestHandlers.DeleteNotification
{
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.Requests.DeleteNotification;
    using EA.Prsd.Core.Mediator;
    using System.Threading.Tasks;

    internal class DeleteExportNotificationHandler : IRequestHandler<DeleteExportNotification, bool>
    {
        private readonly INotificationApplicationRepository repository;

        public DeleteExportNotificationHandler(INotificationApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(DeleteExportNotification message)
        {
            await repository.DeleteExportNotification(message.NotificationId);

            return true;
        }
    }
}
