namespace EA.Iws.RequestHandlers.PackagingType
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.PackagingType;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.PackagingType;

    internal class GetPackagingInfoForNotificationHandler :
        IRequestHandler<GetPackagingInfoForNotification, PackagingData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, PackagingData> packagingMapper;

        public GetPackagingInfoForNotificationHandler(IwsContext context, IMap<NotificationApplication, PackagingData> packagingMapper)
        {
            this.context = context;
            this.packagingMapper = packagingMapper;
        }

        public async Task<PackagingData> HandleAsync(GetPackagingInfoForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            return packagingMapper.Map(notification);
        }
    }
}