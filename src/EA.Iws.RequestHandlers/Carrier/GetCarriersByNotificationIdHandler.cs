namespace EA.Iws.RequestHandlers.Carrier
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Carriers;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Carriers;

    internal class GetCarriersByNotificationIdHandler :
        IRequestHandler<GetCarriersByNotificationId, IEnumerable<CarrierData>>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, IList<CarrierData>> mapper;

        public GetCarriersByNotificationIdHandler(IwsContext context,
            IMap<NotificationApplication, IList<CarrierData>> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CarrierData>> HandleAsync(GetCarriersByNotificationId message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            return mapper.Map(notification);
        }
    }
}