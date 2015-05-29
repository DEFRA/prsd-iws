namespace EA.Iws.RequestHandlers.Shipment
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mediator;
    using Requests.Shipment;
    using PackagingType = Requests.Shipment.PackagingType;

    internal class SetPackagingTypeOnShipmentInfoHandler : IRequestHandler<SetPackagingTypeOnShipmentInfo, Guid>
    {
        private readonly IwsContext db;

        public SetPackagingTypeOnShipmentInfoHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<Guid> HandleAsync(SetPackagingTypeOnShipmentInfo command)
        {
            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);

            var packagingInfos = new List<PackagingInfo>();

            foreach (var packagingType in command.PackagingTypes)
            {
                var packagingInfo = new PackagingInfo();

                switch (packagingType)
                {
                    case PackagingType.Drum:
                        packagingInfo.PackagingType = Domain.PackagingType.Drum;
                        break;
                    case PackagingType.WoodenBarrel:
                        packagingInfo.PackagingType = Domain.PackagingType.WoodenBarrel;
                        break;
                    case PackagingType.Jerrican:
                        packagingInfo.PackagingType = Domain.PackagingType.Jerrican;
                        break;
                    case PackagingType.Box:
                        packagingInfo.PackagingType = Domain.PackagingType.Box;
                        break;
                    case PackagingType.Bag:
                        packagingInfo.PackagingType = Domain.PackagingType.Bag;
                        break;
                    case PackagingType.CompositePackaging:
                        packagingInfo.PackagingType = Domain.PackagingType.CompositePackaging;
                        break;
                    case PackagingType.PressureReceptacle:
                        packagingInfo.PackagingType = Domain.PackagingType.PressureReceptacle;
                        break;
                    case PackagingType.Bulk:
                        packagingInfo.PackagingType = Domain.PackagingType.Bulk;
                        break;
                    case PackagingType.Other:
                        packagingInfo.PackagingType = Domain.PackagingType.Other;
                        packagingInfo.OtherDescription = command.OtherDescription;
                        break;
                    default:
                        throw new InvalidOperationException("Unknown unit type");
                }
                packagingInfos.Add(packagingInfo);
            }

            foreach (var packagingInfo in packagingInfos)
            {
                notification.ShipmentInfo.AddPackagingInfo(packagingInfo);
            }
            
            await db.SaveChangesAsync();

            return notification.Id;
        }
    }
}