namespace EA.Iws.RequestHandlers.Shipment
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Shipment;

    internal class SetPackagingTypeOnShipmentInfoHandler : IRequestHandler<SetPackagingTypeOnShipmentInfo, Guid>
    {
        private readonly IwsContext db;
        private readonly IMap<SetPackagingTypeOnShipmentInfo, IEnumerable<PackagingInfo>> packagingInfoMapper;

        public SetPackagingTypeOnShipmentInfoHandler(IwsContext db,
            IMap<SetPackagingTypeOnShipmentInfo, IEnumerable<PackagingInfo>> packagingInfoMapper)
        {
            this.db = db;
            this.packagingInfoMapper = packagingInfoMapper;
        }

        public async Task<Guid> HandleAsync(SetPackagingTypeOnShipmentInfo command)
        {
            var notification =
                await
                    db.NotificationApplications.Include(n => n.ShipmentInfo)
                        .SingleAsync(n => n.Id == command.NotificationId);

            notification.UpdatePackagingInfo(packagingInfoMapper.Map(command));

            await db.SaveChangesAsync();

            return notification.Id;
        }
    }
}