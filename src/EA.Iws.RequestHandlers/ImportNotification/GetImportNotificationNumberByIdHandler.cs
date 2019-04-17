namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class GetImportNotificationNumberByIdHandler : IRequestHandler<GetImportNotificationNumberById, string>
    {
        private readonly IImportNotificationRepository repository;

        public GetImportNotificationNumberByIdHandler(IImportNotificationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<string> HandleAsync(GetImportNotificationNumberById message)
        {
            return await repository.GetNumber(message.NotificationId);
        }
    }
}
