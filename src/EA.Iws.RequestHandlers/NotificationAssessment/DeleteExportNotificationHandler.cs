namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class DeleteExportNotificationHandler : IRequestHandler<DeleteExportNotification, bool>
    {
        private readonly INotificationApplicationRepository repository;

        public DeleteExportNotificationHandler(INotificationApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(DeleteExportNotification message)
        {
            await repository.Delete(message.NotificationId);

            return true;
        }
    }
}
