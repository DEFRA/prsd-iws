namespace EA.Iws.RequestHandlers.ImportNotification.Producers
{    
    using DataAccess;
    using Domain.ImportNotification;
    using EA.Iws.Domain;
    using EA.Iws.Requests.ImportNotification.Producers;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using System.Threading.Tasks;

    internal class SetProducerDetailsForImportNotificationHandler : IRequestHandler<SetProducerDetailsForImportNotification, Unit>
    {
        private readonly ImportNotificationContext context;
        private readonly IMapper mapper;
        private readonly IProducerRepository producerRepository;
        private readonly ICountryRepository countryRepository;

        public SetProducerDetailsForImportNotificationHandler(IProducerRepository producerRepository,
                                                              ImportNotificationContext context,
                                                              IMapper mapper,
                                                              ICountryRepository countryRepository)
        {
            this.producerRepository = producerRepository;
            this.context = context;
            this.mapper = mapper;
            this.countryRepository = countryRepository;
        }

        public async Task<Unit> HandleAsync(SetProducerDetailsForImportNotification message)
        {
            var producer = await producerRepository.GetByNotificationId(message.ImportNotificationId);
            var contact = mapper.Map<Contact>(message.ProducerDetails.Contact);
            var country = await countryRepository.GetByName(message.ProducerDetails.Address.Country);
            var address = new Address(message.ProducerDetails.Address.AddressLine1,
                                      message.ProducerDetails.Address.AddressLine2,
                                      message.ProducerDetails.Address.TownOrCity,
                                      message.ProducerDetails.Address.PostalCode,
                                      country.Id);

            producer.UpdateProducerDetails(contact, message.ProducerDetails.Name, address);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
