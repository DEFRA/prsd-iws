namespace EA.Iws.Web.Mappings.ImportNotification
{
    using System;
    using Areas.ImportNotification.ViewModels.Shipment;
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mapper;

    public class ShipmentMap : IMapWithParameter<ShipmentViewModel, Guid, Shipment>
    {
        public Shipment Map(ShipmentViewModel source, Guid parameter)
        {
            var shipment = new Shipment(parameter)
            {
                TotalShipments = source.TotalShipments,
                Quantity = source.TotalQuantity,
                Unit = source.Units,
                StartDate = source.StartDate.AsDateTime(),
                EndDate = source.EndDate.AsDateTime()
            };

            return shipment;
        }
    }
}