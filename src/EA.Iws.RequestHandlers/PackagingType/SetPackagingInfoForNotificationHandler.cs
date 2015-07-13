namespace EA.Iws.RequestHandlers.PackagingType
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.PackagingType;

    internal class SetPackagingInfoForNotificationHandler : IRequestHandler<SetPackagingInfoForNotification, Guid>
    {
        private readonly IwsContext context;
        private readonly IMap<SetPackagingInfoForNotification, IEnumerable<PackagingInfo>> packagingInfoMapper;

        public SetPackagingInfoForNotificationHandler(IwsContext context,
            IMap<SetPackagingInfoForNotification, IEnumerable<PackagingInfo>> packagingInfoMapper)
        {
            this.context = context;
            this.packagingInfoMapper = packagingInfoMapper;
        }

        public async Task<Guid> HandleAsync(SetPackagingInfoForNotification command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);

            notification.SetPackagingInfo(packagingInfoMapper.Map(command));

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}