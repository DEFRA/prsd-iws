namespace EA.Iws.Core.Movement.Bulk
{
    public class PrenotificationMovement
    {
        public PrenotificationMovement()
        {
        }

        public string NotificationNumber { get; set; }

        public string ShipmentNumber { get; set; }

        public string Quantity { get; set; }

        public string Unit { get; set; }

        public string PackagingType { get; set; }

        public string ActualDateOfShipment { get; set; }
    }
}
