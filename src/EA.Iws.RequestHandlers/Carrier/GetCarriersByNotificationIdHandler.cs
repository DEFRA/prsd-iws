namespace EA.Iws.RequestHandlers.Carrier
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Carriers;
    using DataAccess;
    using Domain.Notification;
    using Mappings;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Carriers;

    internal class GetCarriersByNotificationIdHandler :
        IRequestHandler<GetCarriersByNotificationId, IEnumerable<CarrierData>>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, IList<CarrierData>> mapper;

        public GetCarriersByNotificationIdHandler(IwsContext context, IMap<NotificationApplication, IList<CarrierData>> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CarrierData>> HandleAsync(GetCarriersByNotificationId message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return mapper.Map(notification);
        }
    }
}