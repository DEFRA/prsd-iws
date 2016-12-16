namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class GetImportNotificationIdByNumberHandler : IRequestHandler<GetImportNotificationIdByNumber, Guid?>
    {
        private readonly IImportNotificationRepository repository;

        public GetImportNotificationIdByNumberHandler(IImportNotificationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Guid?> HandleAsync(GetImportNotificationIdByNumber message)
        {
            return await repository.GetIdOrDefault(message.NotificationNumber);
        }
    }
}
