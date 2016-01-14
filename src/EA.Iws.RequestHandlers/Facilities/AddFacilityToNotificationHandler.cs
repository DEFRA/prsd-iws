namespace EA.Iws.RequestHandlers.Facilities
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Facilities;

    internal class AddFacilityToNotificationHandler : IRequestHandler<AddFacilityToNotification, Guid>
    {
        private readonly IwsContext context;
        private readonly IFacilityRepository facilityRepository;

        public AddFacilityToNotificationHandler(IwsContext context, IFacilityRepository facilityRepository)
        {
            this.context = context;
            this.facilityRepository = facilityRepository;
        }

        public async Task<Guid> HandleAsync(AddFacilityToNotification message)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == message.Address.CountryId);

            var business = ValueObjectInitializer.CreateBusiness(message.Business);
            var address = ValueObjectInitializer.CreateAddress(message.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);

            var facilityCollection = await facilityRepository.GetByNotificationId(message.NotificationId);

            var facility = facilityCollection.AddFacility(business, address, contact);

            await context.SaveChangesAsync();

            return facility.Id;
        }
    }
}