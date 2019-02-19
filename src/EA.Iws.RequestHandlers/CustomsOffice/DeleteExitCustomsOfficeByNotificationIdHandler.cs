namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class DeleteExitCustomsOfficeByNotificationIdHandler :
        IRequestHandler<DeleteExitCustomsOfficeByNotificationId, bool>
    {
        private readonly ITransportRouteRepository repository;

        public DeleteExitCustomsOfficeByNotificationIdHandler(ITransportRouteRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(DeleteExitCustomsOfficeByNotificationId message)
        {
            await repository.DeleteExitCustomsOfficeByNotificationId(message.NotificationId);

            return true;
        }
    }
}
