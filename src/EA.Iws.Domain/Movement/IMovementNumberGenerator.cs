namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;

    public interface IMovementNumberGenerator
    {
        Task<int> Generate(Guid notificationId);
    }
}