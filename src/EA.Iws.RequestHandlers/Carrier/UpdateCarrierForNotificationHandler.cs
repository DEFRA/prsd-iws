namespace EA.Iws.RequestHandlers.Carrier
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Carriers;

    internal class UpdateCarrierForNotificationHandler : IRequestHandler<UpdateCarrierForNotification, Guid>
    {
        private readonly ICarrierRepository repository;
        private readonly IwsContext context;

        public UpdateCarrierForNotificationHandler(IwsContext context,
            ICarrierRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<Guid> HandleAsync(UpdateCarrierForNotification message)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == message.Address.CountryId);

            var carriers = await repository.GetByNotificationId(message.NotificationId);

            var business = ValueObjectInitializer.CreateBusiness(message.Business);
            var address = ValueObjectInitializer.CreateAddress(message.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);

            var carrier = carriers.GetCarrier(message.CarrierId);

            carrier.Address = address;
            carrier.Business = business;
            carrier.Contact = contact;

            await context.SaveChangesAsync();

            return carrier.Id;
        }
    }
}