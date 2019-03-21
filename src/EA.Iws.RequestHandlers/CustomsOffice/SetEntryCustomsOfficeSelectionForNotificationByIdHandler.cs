namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class SetEntryCustomsOfficeSelectionForNotificationByIdHandler : IRequestHandler<SetEntryCustomsOfficeSelectionForNotificationById, bool>
    {
        private readonly IwsContext context;
        private readonly ITransportRouteRepository repository;

        public SetEntryCustomsOfficeSelectionForNotificationByIdHandler(IwsContext context, ITransportRouteRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(SetEntryCustomsOfficeSelectionForNotificationById message)
        {
            var transportRoute = await repository.GetByNotificationId(message.Id);
            var requiredCustomsOffices = new RequiredCustomsOffices();

            if (transportRoute == null)
            {
                transportRoute = new TransportRoute(message.Id);
                context.TransportRoutes.Add(transportRoute);
            }

            bool? existingExit = transportRoute.EntryExitCustomsOfficeSelection != null ? transportRoute.EntryExitCustomsOfficeSelection.Exit : null;

            var selection = new EntryExitCustomsOfficeSelection(message.Selection, existingExit);

            transportRoute.SetEntryExitCustomsOfficeSelection(selection);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
