namespace EA.Iws.RequestHandlers.ImportNotification.Importers
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using EA.Iws.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification.Importers;

    internal class SetImporterDetailsForImportNotificationHandler : IRequestHandler<SetImporterDetailsForImportNotification, Unit>
    {
        private readonly ImportNotificationContext context;
        private readonly IMapper mapper;
        private readonly IImporterRepository importerRepository;
        private readonly ICountryRepository countryRepository;

        public SetImporterDetailsForImportNotificationHandler(IImporterRepository importerRepository, ImportNotificationContext context, IMapper mapper, ICountryRepository countryRepository)
        {
            this.importerRepository = importerRepository;
            this.context = context;
            this.mapper = mapper;
            this.countryRepository = countryRepository;
        }

        public async Task<Unit> HandleAsync(SetImporterDetailsForImportNotification message)
        {
            var importer = await importerRepository.GetByNotificationId(message.ImportNotificationId);
            var contact = mapper.Map<Contact>(message.ImporterDetails.Contact);
            var country = await countryRepository.GetByName(message.ImporterDetails.Address.Country);
            var address = new Address(message.ImporterDetails.Address.AddressLine1,
                                      message.ImporterDetails.Address.AddressLine2,
                                      message.ImporterDetails.Address.TownOrCity,
                                      message.ImporterDetails.Address.PostalCode,
                                      country.Id);

            importer.UpdateImporterDetails(contact, message.ImporterDetails.Name, address);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
