namespace EA.Iws.RequestHandlers.Carrier
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Carriers;
    using DataAccess;
    using Domain.Notification;
    using Mappings;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Carriers;

    internal class GetCarrierForNotificationHandler : IRequestHandler<GetCarrierForNotification, CarrierData>
    {
        private readonly IwsContext context;
        private readonly IMapWithParentObjectId<Carrier, CarrierData> mapper;

        public GetCarrierForNotificationHandler(IwsContext context, IMapWithParentObjectId<Carrier, CarrierData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CarrierData> HandleAsync(GetCarrierForNotification message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);
            var carrier = notification.GetCarrier(message.CarrierId);

            return mapper.Map(carrier, message.NotificationId);
        }
    }
}