namespace EA.Iws.Web.ViewModels.Shared
{
    using System.Collections.Generic;
    using EA.Iws.Core.Shared;

    public class ShipmentAuditTrailViewModel
    {
        public List<ShipmentAuditRecord> ShipmentAuditItems { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int NumberOfShipmentAudits { get; set; }

        public ShipmentAuditTrailViewModel()
        {
            ShipmentAuditItems = new List<ShipmentAuditRecord>();
        }

        public ShipmentAuditTrailViewModel(ShipmentAuditData data)
        {
            ShipmentAuditItems = new List<ShipmentAuditRecord>();
            foreach (var shipmentAudit in data.TableData)
            {
                ShipmentAuditItems.Add(shipmentAudit);
            }

            PageSize = data.PageSize;
            PageNumber = data.PageNumber;
            NumberOfShipmentAudits = data.NumberOfShipmentAudits;
        }
    }
}