namespace EA.Iws.RequestHandlers.Producers
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Producers;

    internal class AddProducerToNotificationHandler : IRequestHandler<AddProducerToNotification, Guid>
    {
        private readonly ICountryRepository countryRepository;
        private readonly IProducerRepository repository;
        private readonly IwsContext context;

        public AddProducerToNotificationHandler(IwsContext context,
            ICountryRepository countryRepository,
            IProducerRepository repository)
        {
            this.context = context;
            this.repository = repository;
            this.countryRepository = countryRepository;
        }

        public async Task<Guid> HandleAsync(AddProducerToNotification command)
        {
            var country = await countryRepository.GetById(command.Address.CountryId.Value);

            var address = ValueObjectInitializer.CreateAddress(command.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(command.Contact);
            var business = ProducerBusiness.CreateProducerBusiness(command.Business.Name, command.Business.BusinessType,
                command.Business.RegistrationNumber, command.Business.OtherDescription);

            var producers = await repository.GetByNotificationId(command.NotificationId);
            var producer = producers.AddProducer(business, address, contact);

            await context.SaveChangesAsync();

            return producer.Id;
        }
    }
}