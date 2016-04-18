namespace EA.Iws.RequestHandlers.Facilities
{
    using System.Threading.Tasks;
    using Core.Facilities;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Facilities;

    internal class GetFacilityForNotificationHandler : IRequestHandler<GetFacilityForNotification, FacilityData>
    {
        private readonly IFacilityRepository facilityRepository;
        private readonly IMapWithParentObjectId<Facility, FacilityData> mapper;

        public GetFacilityForNotificationHandler(IMapWithParentObjectId<Facility, FacilityData> mapper,
            IFacilityRepository facilityRepository)
        {
            this.mapper = mapper;
            this.facilityRepository = facilityRepository;
        }

        public async Task<FacilityData> HandleAsync(GetFacilityForNotification message)
        {
            var facilityCollection = await facilityRepository.GetByNotificationId(message.NotificationId);
            var facility = facilityCollection.GetFacility(message.FacilityId);

            return mapper.Map(facility, message.NotificationId);
        }
    }
}