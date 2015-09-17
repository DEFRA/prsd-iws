namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class SetExitCustomsOfficeForNotificationByIdHandler : IRequestHandler<SetExitCustomsOfficeForNotificationById, CustomsOfficeCompletionStatus>
    {
        private readonly IwsContext context;

        public SetExitCustomsOfficeForNotificationByIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<CustomsOfficeCompletionStatus> HandleAsync(SetExitCustomsOfficeForNotificationById message)
        {
            await context.CheckNotificationAccess(message.Id);

            var transportRoute = await context.TransportRoutes.SingleOrDefaultAsync(p => p.NotificationId == message.Id);

            if (transportRoute == null)
            {
                transportRoute = new TransportRoute(message.Id);
                context.TransportRoutes.Add(transportRoute);
            }

            var country = await context.Countries.SingleAsync(c => c.Id == message.CountryId);

            transportRoute.SetExitCustomsOffice(new ExitCustomsOffice(message.Name, 
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
