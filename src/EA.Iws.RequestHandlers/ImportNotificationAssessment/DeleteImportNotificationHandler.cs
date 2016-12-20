namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class DeleteImportNotificationHandler : IRequestHandler<DeleteImportNotification, bool>
    {
        private readonly IImportNotificationRepository repository;

        public DeleteImportNotificationHandler(IImportNotificationRepository repository)
        {
            this.repository = repository;
        }
        public async Task<bool> HandleAsync(DeleteImportNotification message)
        {
            await repository.Delete(message.NotificationId);

            return true;
        }
    }
}
