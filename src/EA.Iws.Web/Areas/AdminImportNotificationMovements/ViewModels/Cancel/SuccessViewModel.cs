namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Cancel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SuccessViewModel
    {
        public SuccessViewModel(Guid notificationId, List<int> shipmentNumbers)
        {
            NotificationId = notificationId;
            ShipmentNumbers = shipmentNumbers;
        }

        public Guid NotificationId { get; private set; }

        public List<int> ShipmentNumbers { get; private set; }

        public string HeadingText
        {
            get
            {
                if (ShipmentNumbers.Count > 1)
                {
                    return string.Format(CancelResources.MultipleShipmentHeading,
                        string.Join(", ",
                            ShipmentNumbers.Take(ShipmentNumbers.Count - 1)),
                        ShipmentNumbers.Last());
                }

                return string.Format(CancelResources.SingleShipmentHeading,
                    ShipmentNumbers.First());
            }
        }
    }
}