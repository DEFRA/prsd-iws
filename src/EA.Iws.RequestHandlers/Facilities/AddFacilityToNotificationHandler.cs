namespace EA.Iws.RequestHandlers.Facilities
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Facilities;

    internal class AddFacilityToNotificationHandler : IRequestHandler<AddFacilityToNotification, Guid>
    {
        private readonly IwsContext context;

        public AddFacilityToNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(AddFacilityToNotification message)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == message.Address.CountryId);

            var business = ValueObjectInitializer.CreateBusiness(message.Business);
            var address = ValueObjectInitializer.CreateAddress(message.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);

            var notification = await context.GetNotificationApplication(message.NotificationId);

            var facility = notification.AddFacility(business, address, contact);

            await context.SaveChangesAsync();

            return facility.Id;
        }
    }
}