namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class SetEntryCustomsOfficeForNotificationByIdHandler : IRequestHandler<SetEntryCustomsOfficeForNotificationById, CustomsOfficeCompletionStatus>
    {
        private readonly IwsContext context;
        private readonly ITransportRouteRepository repository;

        public SetEntryCustomsOfficeForNotificationByIdHandler(IwsContext context, ITransportRouteRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<CustomsOfficeCompletionStatus> HandleAsync(SetEntryCustomsOfficeForNotificationById message)
        {
            var transportRoute = await repository.GetByNotificationId(message.Id);

            if (transportRoute == null)
            {
                transportRoute = new TransportRoute(message.Id);
                context.TransportRoutes.Add(transportRoute);
            }

            var country = await context.Countries.SingleAsync(c => c.Id == message.CountryId);

            transportRoute.SetEntryCustomsOffice(new EntryCustomsOffice(message.Name, 
                message.Address, 
                country));

            await context.SaveChangesAsync();

            return new CustomsOfficeCompletionStatus
            {
                CustomsOfficesRequired = transportRoute.GetRequiredCustomsOffices()
            };
        }
    }
}
