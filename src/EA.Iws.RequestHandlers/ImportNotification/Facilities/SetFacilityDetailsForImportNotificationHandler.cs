namespace EA.Iws.RequestHandlers.ImportNotification.Facilities
{
    using EA.Iws.DataAccess;
    using EA.Iws.Domain;
    using EA.Iws.Domain.ImportNotification;
    using EA.Iws.Requests.ImportNotification.Facilities;
    using EA.Prsd.Core.Mapper;
    using EA.Prsd.Core.Mediator;
    using System.Linq;
    using System.Threading.Tasks;

    internal class SetFacilityDetailsForImportNotificationHandler : IRequestHandler<SetFacilityDetailsForImportNotification, Unit>
    {
        private readonly ImportNotificationContext context;
        private readonly IMapper mapper;
        private readonly IFacilityRepository facilityRepository;
        private readonly ICountryRepository countryRepository;

        public SetFacilityDetailsForImportNotificationHandler(IFacilityRepository facilityRepository,
                                                              ImportNotificationContext context,
                                                              IMapper mapper,
                                                              ICountryRepository countryRepository)
        {
            this.facilityRepository = facilityRepository;
            this.context = context;
            this.mapper = mapper;
            this.countryRepository = countryRepository;
        }

        public async Task<Unit> HandleAsync(SetFacilityDetailsForImportNotification message)
        {
            var facility = await facilityRepository.GetByNotificationId(message.ImportNotificationId);
            var facilityData = facility.Facilities.SingleOrDefault();

            var contact = mapper.Map<Contact>(message.FacilityDetails.Contact);
            var country = await countryRepository.GetByName(message.FacilityDetails.Address.Country);
            var address = new Address(message.FacilityDetails.Address.AddressLine1,
                                      message.FacilityDetails.Address.AddressLine2,
                                      message.FacilityDetails.Address.TownOrCity,
                                      message.FacilityDetails.Address.PostalCode,
                                      country.Id);

            facilityData.UpdateFacilityDetails(contact, message.FacilityDetails.Name, address);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
