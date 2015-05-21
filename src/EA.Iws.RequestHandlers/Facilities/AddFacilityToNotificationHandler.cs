namespace EA.Iws.RequestHandlers.Facilities
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Facilities;

    public class AddFacilityToNotificationHandler : IRequestHandler<AddFacilityToNotification, Guid>
    {
        private readonly IwsContext context;

        public AddFacilityToNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(AddFacilityToNotification message)
        {
            var notification = await context.NotificationApplications.FindAsync(message.Facility.NotificationId);

            if (notification == null)
            {
                throw new InvalidOperationException("Attempted to add a facility to a missing notification");
            }

            var country = await context.Countries.SingleAsync(c => c.Id == message.Facility.Address.CountryId);

            var address = ValueObjectInitializer.CreateAddress(message.Facility.Address, country.Name);

            var contact = ValueObjectInitializer.CreateContact(message.Facility.Contact);

            var business = ValueObjectInitializer.CreateBusiness(message.Facility.Business);

            var facility = new Facility(business, address, contact, country, message.Facility.IsActualSiteOfTreatment);

            notification.AddFacility(facility);

            await context.SaveChangesAsync();

            return facility.Id;
        }
    }
}
