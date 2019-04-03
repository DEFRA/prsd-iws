namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class DeleteEntryCustomsOfficeByNotificationIdHandler :
        IRequestHandler<DeleteEntryCustomsOfficeByNotificationId, bool>
    {
        private readonly ITransportRouteRepository repository;

        public DeleteEntryCustomsOfficeByNotificationIdHandler(ITransportRouteRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(DeleteEntryCustomsOfficeByNotificationId message)
        {
            await repository.DeleteEntryCustomsOfficeByNotificationId(message.NotificationId);

            return true;
        }
    }
}
