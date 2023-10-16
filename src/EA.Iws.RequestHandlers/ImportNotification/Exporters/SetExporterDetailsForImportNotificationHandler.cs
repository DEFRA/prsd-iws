namespace EA.Iws.RequestHandlers.ImportNotification.Exporters
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using EA.Iws.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification.Exporters;

    internal class SetExporterDetailsForImportNotificationHandler : IRequestHandler<SetExporterDetailsForImportNotification, Unit>
    {
        private readonly IExporterRepository exporterRepository;
        private readonly ImportNotificationContext context;
        private readonly IMapper mapper;
        private readonly ICountryRepository countryRepository;

        public SetExporterDetailsForImportNotificationHandler(IExporterRepository exporterRepository, ImportNotificationContext context, IMapper mapper, ICountryRepository countryRepository)
        {
            this.exporterRepository = exporterRepository;
            this.context = context;
            this.mapper = mapper;
            this.countryRepository = countryRepository;
        }

        public async Task<Unit> HandleAsync(SetExporterDetailsForImportNotification message)
        {
            var exporter = await exporterRepository.GetByNotificationId(message.ImportNotificationId);
            var contact = mapper.Map<Contact>(message.ExporterDetails.Contact);
            var country = await countryRepository.GetByName(message.ExporterDetails.Address.Country);
            var address = new Address(message.ExporterDetails.Address.AddressLine1,
                                      message.ExporterDetails.Address.AddressLine2,
                                      message.ExporterDetails.Address.TownOrCity,
                                      message.ExporterDetails.Address.PostalCode,
                                      country.Id);

            exporter.UpdateExporterDetails(contact, message.ExporterDetails.Name, address);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
