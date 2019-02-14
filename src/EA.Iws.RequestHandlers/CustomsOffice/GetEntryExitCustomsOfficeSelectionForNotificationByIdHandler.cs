namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Threading.Tasks;
    using Core.CustomsOffice;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class GetEntryExitCustomsOfficeSelectionForNotificationByIdHandler :
        IRequestHandler<GetEntryExitCustomsOfficeSelectionForNotificationById, EntryExitCustomsOfficeSelectionData>
    {
        private readonly ITransportRouteRepository repository;

        public GetEntryExitCustomsOfficeSelectionForNotificationByIdHandler(ITransportRouteRepository repository)
        {
            this.repository = repository;
        }

        public async Task<EntryExitCustomsOfficeSelectionData> HandleAsync(GetEntryExitCustomsOfficeSelectionForNotificationById message)
        {
            var transportRoute = await repository.GetByNotificationId(message.Id);

            if (transportRoute == null || transportRoute.EntryExitCustomsOfficeSelection == null)
            {
                return null;
            }

            return new EntryExitCustomsOfficeSelectionData(transportRoute.EntryExitCustomsOfficeSelection.Entry, transportRoute.EntryExitCustomsOfficeSelection.Exit);
        }
    }
}