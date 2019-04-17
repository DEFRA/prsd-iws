namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.ShipmentAudit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EA.Iws.Core.Shared;

    public class ShipmentAuditViewModel
    {
        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public List<ShipmentAuditRecord> ShipmentAuditItems { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int NumberOfShipmentAudits { get; set; }

        public int NumberOfShipments
        {
            get
            {
                return ShipmentAuditItems.Select(x => x.ShipmentNumber).Distinct().Count();
            }
          }

        public ShipmentAuditViewModel()
        {
            ShipmentAuditItems = new List<ShipmentAuditRecord>();
        }

        public ShipmentAuditViewModel(ShipmentAuditData data)
        {
            ShipmentAuditItems = new List<ShipmentAuditRecord>();
            foreach (ShipmentAuditRecord shipmentAudit in data.TableData)
            {
                ShipmentAuditItems.Add(shipmentAudit);
            }

            PageSize = data.PageSize;
            PageNumber = data.PageNumber;
            NumberOfShipmentAudits = data.NumberOfShipmentAudits;
        }
    }
}