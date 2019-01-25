namespace EA.Iws.Core.Movement.Bulk
{
    using System.Collections.Generic;

    public class PrenotificationMovementCollection
    {
        public PrenotificationMovementCollection()
        {
            PrenotificationMovement obj1 = new PrenotificationMovement()
            {
                NotificationNumber = string.Empty,
                ShipmentNumber = "1",
                Quantity = string.Empty,
                Unit = string.Empty,
                PackagingType = string.Empty,
                ActualDateOfShipment = string.Empty
            };
            ObjectsList.Add(obj1);
            PrenotificationMovement obj2 = new PrenotificationMovement()
            {
                NotificationNumber = string.Empty,
                ShipmentNumber = "1",
                Quantity = string.Empty,
                Unit = string.Empty,
                PackagingType = string.Empty,
                ActualDateOfShipment = string.Empty
            };
            ObjectsList.Add(obj1);
        }

        public List<PrenotificationMovement> ObjectsList = new List<PrenotificationMovement>();
    }
}
