namespace EA.Iws.RequestHandlers.Shipment
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
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
            var notification = await db.NotificationApplications.Include(n => n.ShipmentInfo).SingleAsync(n => n.Id == command.NotificationId);

            var packagingTypes = new List<Domain.Notification.PackagingType>();

            foreach (var selectedPackagingType in command.PackagingTypes)
            {
                switch (selectedPackagingType)
                {
                    case PackagingType.Drum:
                        packagingTypes.Add(Domain.Notification.PackagingType.Drum);
                        break;
                    case PackagingType.WoodenBarrel:
                        packagingTypes.Add(Domain.Notification.PackagingType.WoodenBarrel);
                        break;
                    case PackagingType.Jerrican:
                        packagingTypes.Add(Domain.Notification.PackagingType.Jerrican);
                        break;
                    case PackagingType.Box:
                        packagingTypes.Add(Domain.Notification.PackagingType.Box);
                        break;
                    case PackagingType.Bag:
                        packagingTypes.Add(Domain.Notification.PackagingType.Bag);
                        break;
                    case PackagingType.CompositePackaging:
                        packagingTypes.Add(Domain.Notification.PackagingType.CompositePackaging);
                        break;
                    case PackagingType.PressureReceptacle:
                        packagingTypes.Add(Domain.Notification.PackagingType.PressureReceptacle);
                        break;
                    case PackagingType.Bulk:
                        packagingTypes.Add(Domain.Notification.PackagingType.Bulk);
                        break;
                    case PackagingType.Other:
                        packagingTypes.Add(Domain.Notification.PackagingType.Other);
                        break;
                    default:
                        throw new InvalidOperationException("Unknown unit type");
                }
            }

            foreach (var packagingType in packagingTypes)
            {
                if (packagingType == Domain.Notification.PackagingType.Other)
                {
                    notification.AddPackagingInfo(packagingType, command.OtherDescription);
                }
                else
                {
                    notification.AddPackagingInfo(packagingType);
                }
            }
            
            await db.SaveChangesAsync();

            return notification.Id;
        }
    }
}