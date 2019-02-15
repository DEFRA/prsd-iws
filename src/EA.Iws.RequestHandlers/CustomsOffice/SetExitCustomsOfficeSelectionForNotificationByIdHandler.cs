namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class SetExitCustomsOfficeSelectionForNotificationByIdHandler : IRequestHandler<SetExitCustomsOfficeSelectionForNotificationById, bool>
    {
        private readonly IwsContext context;
        private readonly ITransportRouteRepository repository;

        public SetExitCustomsOfficeSelectionForNotificationByIdHandler(IwsContext context, ITransportRouteRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(SetExitCustomsOfficeSelectionForNotificationById message)
        {
            var transportRoute = await repository.GetByNotificationId(message.Id);
            var requiredCustomsOffices = new RequiredCustomsOffices();

            if (transportRoute == null)
            {
                transportRoute = new TransportRoute(message.Id);
                context.TransportRoutes.Add(transportRoute);
            }

            bool? existingEntry = transportRoute.EntryExitCustomsOfficeSelection != null ? transportRoute.EntryExitCustomsOfficeSelection.Entry : false;

            var selection = new EntryExitCustomsOfficeSelection(existingEntry, message.Selection);

            transportRoute.SetEntryExitCustomsOfficeSelection(selection);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
