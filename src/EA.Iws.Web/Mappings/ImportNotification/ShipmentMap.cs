namespace EA.Iws.Web.Mappings.ImportNotification
{
    using Areas.ImportNotification.ViewModels.Shipment;
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mapper;

    public class ShipmentMap : IMap<ShipmentViewModel, Shipment>
    {
        public Shipment Map(ShipmentViewModel source)
        {
            var shipment = new Shipment(source.ImportNotificationId);

            shipment.TotalShipments = source.TotalShipments;
            shipment.Quantity = source.TotalQuantity;
            shipment.Unit = source.Units;
            shipment.StartDate = source.StartDate.AsDateTime();
            shipment.EndDate = source.EndDate.AsDateTime();

            return shipment;
        }
    }
}