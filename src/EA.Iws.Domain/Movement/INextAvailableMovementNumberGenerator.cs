namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;

    public interface INextAvailableMovementNumberGenerator
    {
        Task<int> GetNext(Guid notificationId);
    }
}
