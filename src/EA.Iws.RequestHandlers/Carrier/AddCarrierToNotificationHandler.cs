namespace EA.Iws.RequestHandlers.Carrier
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Carriers;

    internal class AddCarrierToNotificationHandler : IRequestHandler<AddCarrierToNotification, Guid>
    {
        private readonly ICountryRepository countryRepository;
        private readonly ICarrierRepository repository;
        private readonly IwsContext context;

        public AddCarrierToNotificationHandler(IwsContext context,
            ICarrierRepository repository, ICountryRepository countryRepository)
        {
            this.context = context;
            this.repository = repository;
            this.countryRepository = countryRepository;
        }

        public async Task<Guid> HandleAsync(AddCarrierToNotification command)
        {
            var carriers = await repository.GetByNotificationId(command.NotificationId);

            var country = await countryRepository.GetById(command.Address.CountryId.Value);
            var business = ValueObjectInitializer.CreateBusiness(command.Business);
            var address = ValueObjectInitializer.CreateAddress(command.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(command.Contact);
            var carrier = carriers.AddCarrier(business, address, contact);

            await context.SaveChangesAsync();

            return carrier.Id;
        }
    }
}