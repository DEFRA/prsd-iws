namespace EA.Iws.RequestHandlers.Producers
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Producers;

    internal class UpdateProducerForNotificationHandler : IRequestHandler<UpdateProducerForNotification, Guid>
    {
        private readonly ICountryRepository countryRepository;
        private readonly IProducerRepository repository;
        private readonly IwsContext context;

        public UpdateProducerForNotificationHandler(IwsContext context,
            ICountryRepository countryRepository,
            IProducerRepository repository)
        {
            this.context = context;
            this.repository = repository;
            this.countryRepository = countryRepository;
        }

        public async Task<Guid> HandleAsync(UpdateProducerForNotification message)
        {
            var country = await countryRepository.GetById(message.Address.CountryId.Value);
            var producers = await repository.GetByNotificationId(message.NotificationId);

            var business = ProducerBusiness.CreateProducerBusiness(message.Business.Name,
                BusinessType.FromBusinessType(message.Business.BusinessType),
                message.Business.RegistrationNumber,
                message.Business.OtherDescription);
            var address = ValueObjectInitializer.CreateAddress(message.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);

            var producer = producers.GetProducer(message.ProducerId);

            producer.Address = address;
            producer.Business = business;
            producer.Contact = contact;

            await context.SaveChangesAsync();

            return producer.Id;
        }
    }
}