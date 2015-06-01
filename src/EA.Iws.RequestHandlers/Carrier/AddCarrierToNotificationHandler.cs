namespace EA.Iws.RequestHandlers.Carrier
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Carriers;

    internal class AddCarrierToNotificationHandler : IRequestHandler<AddCarrierToNotification, Guid>
    {
        private readonly IwsContext context;

        public AddCarrierToNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(AddCarrierToNotification command)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);

            var country = await context.Countries.SingleAsync(c => c.Id == command.Address.CountryId);
            var business = ValueObjectInitializer.CreateBusiness(command.Business);
            var address = ValueObjectInitializer.CreateAddress(command.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(command.Contact);
            var carrier = notification.AddCarrier(business, address, contact);

            notification.AddShipmentInfo();

            await context.SaveChangesAsync();

            return carrier.Id;
        }
    }
}