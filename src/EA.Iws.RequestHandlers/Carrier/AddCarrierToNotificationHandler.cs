namespace EA.Iws.RequestHandlers.Carrier
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
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
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == command.CarrierData.NotificationId);

            var country = await context.Countries.SingleAsync(c => c.Id == command.CarrierData.Address.CountryId);
            var business = ValueObjectInitializer.CreateBusiness(command.CarrierData.Business);
            var address = ValueObjectInitializer.CreateAddress(command.CarrierData.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(command.CarrierData.Contact);
            var carrier = new Carrier(business, address, contact);
            notification.AddCarrier(carrier);

            await context.SaveChangesAsync();

            return carrier.Id;
        }
    }
}
