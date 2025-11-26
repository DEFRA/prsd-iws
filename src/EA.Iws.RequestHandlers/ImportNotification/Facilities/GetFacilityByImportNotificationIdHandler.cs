namespace EA.Iws.RequestHandlers.ImportNotification.Facilities
{
    using EA.Iws.Domain.ImportNotification;
    using EA.Iws.Requests.ImportNotification.Facilities;
    using EA.Prsd.Core.Mapper;
    using EA.Prsd.Core.Mediator;
    using System.Linq;
    using System.Threading.Tasks;

    internal class GetFacilityByImportNotificationIdHandler : IRequestHandler<GetFacilityByImportNotificationId, Core.ImportNotification.Summary.Facility>
    {
        private readonly IFacilityRepository facilityRepository;
        private readonly IMapper mapper;

        public GetFacilityByImportNotificationIdHandler(IFacilityRepository facilityRepository, IMapper mapper)
        {
            this.facilityRepository = facilityRepository;
            this.mapper = mapper;
        }

        public async Task<Core.ImportNotification.Summary.Facility> HandleAsync(GetFacilityByImportNotificationId message)
        {
            var facility = await facilityRepository.GetByNotificationId(message.ImportNotificationId);
            
            return mapper.Map<Core.ImportNotification.Summary.Facility>(facility.Facilities.FirstOrDefault());
        }
    }
}
