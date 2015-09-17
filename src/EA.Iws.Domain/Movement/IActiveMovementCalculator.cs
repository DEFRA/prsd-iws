namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;

    public interface IActiveMovementCalculator
    {
        Task<int> TotalActiveMovementsAsync(Guid notificationId);
    }
}
