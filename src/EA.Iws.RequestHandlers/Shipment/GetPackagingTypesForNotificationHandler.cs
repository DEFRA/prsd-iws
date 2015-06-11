namespace EA.Iws.RequestHandlers.Shipment
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Shipment;

    internal class GetPackagingTypesForNotificationHandler :
        IRequestHandler<GetPackagingTypesForNotification, PackagingData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, PackagingData> packagingMapper;

        public GetPackagingTypesForNotificationHandler(IwsContext context, IMap<NotificationApplication, PackagingData> packagingMapper)
        {
            this.context = context;
            this.packagingMapper = packagingMapper;
        }

        public async Task<PackagingData> HandleAsync(GetPackagingTypesForNotification message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return packagingMapper.Map(notification);
        }
    }
}