namespace EA.Iws.Core.ImportMovement
{
    using EA.Iws.Core.Movement;
    using Shared;

    public class AddedCancellableImportMovementValidation
    {
        public bool IsCancellableExistingShipment { get; set; }

        public bool IsNonCancellableExistingShipment { get; set; }

        public MovementStatus Status { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}
