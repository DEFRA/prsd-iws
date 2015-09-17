namespace EA.Iws.Core.MovementReceipt
{
    using Shared;

    public class MovementReceiptQuantityData
    {
        public ShipmentQuantityUnits MovementUnit { get; set; }

        public ShipmentQuantityUnits NotificationUnits { get; set; }

        public decimal? Quantity { get; set; }
    }
}
