namespace EA.Iws.Domain.MovementReceipt
{
    using System;
    using System.Threading.Tasks;

    public interface IMovementQuantityCalculator
    {
        Task<decimal> TotalQuantityReceivedAsync(Guid notificationId);
        Task<decimal> TotalQuantityRemainingAsync(Guid notificationId);
    }
}
