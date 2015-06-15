namespace EA.Iws.RequestHandlers.Shipment
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mediator;
    using Requests.Shipment;

    internal class CreateNumberOfShipmentsInfoHandler : IRequestHandler<CreateNumberOfShipmentsInfo, Guid>
    {
        private readonly IwsContext db;

        public CreateNumberOfShipmentsInfoHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<Guid> HandleAsync(CreateNumberOfShipmentsInfo command)
        {
            ShipmentQuantityUnits unit;

            switch (command.Units)
            {
                case Requests.Notification.ShipmentQuantityUnits.Tonnes:
                    unit = ShipmentQuantityUnits.Tonnes;
                    break;
                case Requests.Notification.ShipmentQuantityUnits.CubicMetres:
                    unit = ShipmentQuantityUnits.CubicMetres;
                    break;
                case Requests.Notification.ShipmentQuantityUnits.Litres:
                    unit = ShipmentQuantityUnits.Litres;
                    break;
                case Requests.Notification.ShipmentQuantityUnits.Kilograms:
                    unit = ShipmentQuantityUnits.Kilogram;
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown unit: {0}", command.Units));
            }

            var notification = await db.NotificationApplications.Include(n => n.ShipmentInfo).SingleAsync(n => n.Id == command.NotificationId);
            notification.AddShipmentInfo(command.StartDate, command.EndDate, command.NumberOfShipments, command.Quantity, unit);

            await db.SaveChangesAsync();

            return notification.Id;
        }
    }
}