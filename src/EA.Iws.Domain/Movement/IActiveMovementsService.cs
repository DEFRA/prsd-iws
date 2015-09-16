namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;

    public interface IActiveMovementsService
    {
        Task<int> TotalActiveMovementsAsync(Guid notificationId);
        bool IsMovementActive(Movement movement);
    }
}
