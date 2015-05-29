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

            var packagingTypes = new List<Domain.PackagingType>();

            foreach (var selectedPackagingType in command.PackagingTypes)
            {
                switch (selectedPackagingType)
                {
                    case PackagingType.Drum:
                        packagingTypes.Add(Domain.PackagingType.Drum);
                        break;
                    case PackagingType.WoodenBarrel:
                        packagingTypes.Add(Domain.PackagingType.WoodenBarrel);
                        break;
                    case PackagingType.Jerrican:
                        packagingTypes.Add(Domain.PackagingType.Jerrican);
                        break;
                    case PackagingType.Box:
                        packagingTypes.Add(Domain.PackagingType.Box);
                        break;
                    case PackagingType.Bag:
                        packagingTypes.Add(Domain.PackagingType.Bag);
                        break;
                    case PackagingType.CompositePackaging:
                        packagingTypes.Add(Domain.PackagingType.CompositePackaging);
                        break;
                    case PackagingType.PressureReceptacle:
                        packagingTypes.Add(Domain.PackagingType.PressureReceptacle);
                        break;
                    case PackagingType.Bulk:
                        packagingTypes.Add(Domain.PackagingType.Bulk);
                        break;
                    case PackagingType.Other:
                        packagingTypes.Add(Domain.PackagingType.Other);
                        break;
                    default:
                        throw new InvalidOperationException("Unknown unit type");
                }
            }

            foreach (var packagingType in packagingTypes)
            {
                if (packagingType == Domain.PackagingType.Other)
                {
                    notification.ShipmentInfo.AddPackagingInfo(packagingType, command.OtherDescription);
                }
                else
                {
                    notification.ShipmentInfo.AddPackagingInfo(packagingType);
                }
            }
            
            await db.SaveChangesAsync();

            return notification.Id;
        }
    }
}