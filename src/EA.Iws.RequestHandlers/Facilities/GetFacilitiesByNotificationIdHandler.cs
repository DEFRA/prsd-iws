namespace EA.Iws.RequestHandlers.Facilities
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Facilities;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Facilities;

    internal class GetFacilitiesByNotificationIdHandler : IRequestHandler<GetFacilitiesByNotificationId, IList<FacilityData>>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, IList<FacilityData>> mapper;

        public GetFacilitiesByNotificationIdHandler(IwsContext context, IMap<NotificationApplication, IList<FacilityData>> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IList<FacilityData>> HandleAsync(GetFacilitiesByNotificationId message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return mapper.Map(notification);
        }
    }
}