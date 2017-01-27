namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class AddProducerHandler : IRequestHandler<AddProducer, Guid>
    {
        private readonly IwsContext context;
        private readonly ICountryRepository countryRepository;
        private readonly IProducerRepository repository;

        public AddProducerHandler(IwsContext context,
            ICountryRepository countryRepository,
            IProducerRepository repository)
        {
            this.context = context;
            this.countryRepository = countryRepository;
            this.repository = repository;
        }

        public async Task<Guid> HandleAsync(AddProducer message)
        {
            var country = await countryRepository.GetById(message.Address.CountryId.Value);

            var address = ValueObjectInitializer.CreateAddress(message.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);
            var business = ProducerBusiness.CreateProducerBusiness(message.Business.Name, message.Business.BusinessType,
                message.Business.RegistrationNumber, message.Business.OtherDescription);

            var producers = await repository.GetByNotificationId(message.NotificationId);
            var producer = producers.AddProducer(business, address, contact);

            await context.SaveChangesAsync();

            return producer.Id;
        }
    }
}