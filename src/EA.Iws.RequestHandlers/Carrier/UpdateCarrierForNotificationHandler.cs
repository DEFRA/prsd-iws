namespace EA.Iws.RequestHandlers.Carrier
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Carriers;

    internal class UpdateCarrierForNotificationHandler : IRequestHandler<UpdateCarrierForNotification, Guid>
    {
        private readonly IwsContext context;

        public UpdateCarrierForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(UpdateCarrierForNotification message)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == message.Address.CountryId);

            var notification = await context.GetNotificationApplication(message.NotificationId);

            var business = ValueObjectInitializer.CreateBusiness(message.Business);
            var address = ValueObjectInitializer.CreateAddress(message.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);

            var carrier = notification.GetCarrier(message.CarrierId);

            carrier.Address = address;
            carrier.Business = business;
            carrier.Contact = contact;

            await context.SaveChangesAsync();

            return carrier.Id;
        }
    }
}