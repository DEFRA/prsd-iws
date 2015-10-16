namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class CreateImportNotificationHandler : IRequestHandler<CreateImportNotification, Guid>
    {
        private readonly IImportNotificationRepository importNotificationRepository;
        private readonly IwsContext context;

        public CreateImportNotificationHandler(IImportNotificationRepository importNotificationRepository, IwsContext context)
        {
            this.importNotificationRepository = importNotificationRepository;
            this.context = context;
        }

        public async Task<Guid> HandleAsync(CreateImportNotification message)
        {
            var notification = new ImportNotification(message.NotificationType, message.Number);

            await importNotificationRepository.Add(notification);

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}
