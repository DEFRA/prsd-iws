namespace EA.Iws.Requests.Movement
{
    using Core.Shared;

    public class MovementQuantityData
    {
        public decimal TotalNotifiedQuantity { get; set; }

        public decimal TotalCurrentlyUsedQuantity { get; set; }

        public decimal? ThisMovementQuantity { get; set; }

        public ShipmentQuantityUnits Units { get; set; }

        public decimal AvailableQuantity 
        {
            get { return TotalNotifiedQuantity - TotalCurrentlyUsedQuantity; }
        }
    }
}
