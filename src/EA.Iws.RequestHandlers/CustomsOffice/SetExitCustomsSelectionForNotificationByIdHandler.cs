namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class SetExitCustomsSelectionForNotificationByIdHandler : IRequestHandler<SetExitCustomsSelectionForNotificationById, bool>
    {
        private readonly IwsContext context;
        private readonly ITransportRouteRepository repository;

        public SetExitCustomsSelectionForNotificationByIdHandler(IwsContext context, ITransportRouteRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(SetExitCustomsSelectionForNotificationById message)
        {
            var transportRoute = await repository.GetByNotificationId(message.Id);
            var requiredCustomsOffices = new RequiredCustomsOffices();

            if (transportRoute == null)
            {
                transportRoute = new TransportRoute(message.Id);
                context.TransportRoutes.Add(transportRoute);
            }

            bool existingEntry = transportRoute.EntryExitCustomsSelection != null ? transportRoute.EntryExitCustomsSelection.Entry : false;

            var selection = new EntryExitCustomsSelection(existingEntry, message.ExitSelection);

            transportRoute.SetEntryExitCustomsSelection(selection);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
