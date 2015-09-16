namespace EA.Iws.Domain.MovementReceipt
{
    using Movement;

    public interface IMovementReceiptService
    {
        bool IsReceived(Movement movement);
    }
}
