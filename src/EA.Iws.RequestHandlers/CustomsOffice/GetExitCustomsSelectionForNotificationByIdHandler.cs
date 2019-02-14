namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Threading.Tasks;
    using Core.CustomsOffice;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class GetExitCustomsSelectionForNotificationByIdHandler :
        IRequestHandler<GetEntryExitCustomsSelectionForNotificationById, EntryExitCustomsSelectionData>
    {
        private readonly ITransportRouteRepository repository;

        public GetExitCustomsSelectionForNotificationByIdHandler(ITransportRouteRepository repository)
        {
            this.repository = repository;
        }

        public async Task<EntryExitCustomsSelectionData> HandleAsync(GetEntryExitCustomsSelectionForNotificationById message)
        {
            var transportRoute = await repository.GetByNotificationId(message.Id);

            if (transportRoute == null || transportRoute.EntryExitCustomsSelection == null)
            {
                return null;
            }

            return new EntryExitCustomsSelectionData(transportRoute.EntryExitCustomsSelection.Entry, transportRoute.EntryExitCustomsSelection.Exit);
        }
    }
}