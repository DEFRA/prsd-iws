﻿namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.Cancel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SuccessViewModel
    {
        private const string SingleShipmentHeading = "You've now cancelled shipment {0}";
        private const string MultiShipmentsHeading = "You've now cancelled shipments {0} and {1}";

        public Guid NotificationId { get; private set; }

        public List<int> ShipmentNumbers { get; private set; }

        public string HeadingText
        {
            get
            {
                if (ShipmentNumbers.Count > 1)
                {
                    return string.Format(MultiShipmentsHeading,
                        string.Join(", ", 
                            ShipmentNumbers.Take(ShipmentNumbers.Count - 1)), 
                        ShipmentNumbers.Last());
                }

                return string.Format(SingleShipmentHeading, 
                    ShipmentNumbers.First());
            }
        }

        public SuccessViewModel(Guid notificationId, IEnumerable<int> shipmentNumbers)
        {
            NotificationId = notificationId;
            ShipmentNumbers = shipmentNumbers.OrderBy(x => x).ToList();
        }
    }
}