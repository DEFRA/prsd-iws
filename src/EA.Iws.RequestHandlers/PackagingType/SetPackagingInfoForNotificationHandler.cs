namespace EA.Iws.RequestHandlers.PackagingType
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.PackagingType;

    internal class SetPackagingInfoForNotificationHandler : IRequestHandler<SetPackagingInfoForNotification, Guid>
    {
        private readonly IwsContext db;
        private readonly IMap<SetPackagingInfoForNotification, IEnumerable<PackagingInfo>> packagingInfoMapper;

        public SetPackagingInfoForNotificationHandler(IwsContext db,
            IMap<SetPackagingInfoForNotification, IEnumerable<PackagingInfo>> packagingInfoMapper)
        {
            this.db = db;
            this.packagingInfoMapper = packagingInfoMapper;
        }

        public async Task<Guid> HandleAsync(SetPackagingInfoForNotification command)
        {
            var notification =
                await
                    db.NotificationApplications.Include(n => n.ShipmentInfo)
                        .SingleAsync(n => n.Id == command.NotificationId);

            notification.SetPackagingInfo(packagingInfoMapper.Map(command));

            await db.SaveChangesAsync();

            return notification.Id;
        }
    }
}