namespace EA.Iws.RequestHandlers.Facilities
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Facilities;
    using DataAccess;
    using Domain.Notification;
    using Mappings;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Facilities;

    internal class GetFacilityForNotificationHandler : IRequestHandler<GetFacilityForNotification, FacilityData>
    {
        private readonly IwsContext context;
        private readonly IMapWithParentObjectId<Facility, FacilityData> mapper;

        public GetFacilityForNotificationHandler(IwsContext context, IMapWithParentObjectId<Facility, FacilityData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<FacilityData> HandleAsync(GetFacilityForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);
            var facility = notification.GetFacility(message.FacilityId);

            return mapper.Map(facility, message.NotificationId);
        }
    }
}