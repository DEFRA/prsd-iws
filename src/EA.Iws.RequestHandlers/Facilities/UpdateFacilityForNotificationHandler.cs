namespace EA.Iws.RequestHandlers.Facilities
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Facilities;

    internal class UpdateFacilityForNotificationHandler : IRequestHandler<UpdateFacilityForNotification, Guid>
    {
        private readonly IwsContext context;

        public UpdateFacilityForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(UpdateFacilityForNotification message)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == message.Address.CountryId);

            var notification = await context.GetNotificationApplication(message.NotificationId);

            var business = ValueObjectInitializer.CreateBusiness(message.Business);
            var address = ValueObjectInitializer.CreateAddress(message.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);

            var facility = notification.GetFacility(message.FacilityId);

            facility.Address = address;
            facility.Business = business;
            facility.Contact = contact;

            await context.SaveChangesAsync();

            return facility.Id;
        }
    }
}