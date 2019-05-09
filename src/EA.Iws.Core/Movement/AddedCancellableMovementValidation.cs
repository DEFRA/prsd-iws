namespace EA.Iws.Core.Movement
{
    public class AddedCancellableMovementValidation
    {
        public bool IsCancellableExistingShipment { get; set; }

        public bool IsNonCancellableExistingShipment { get; set; }

        public MovementStatus Status { get; set; }
    }
}
