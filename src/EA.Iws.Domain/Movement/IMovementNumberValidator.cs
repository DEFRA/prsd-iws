namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;

    public interface IMovementNumberValidator
    {
        Task<bool> Validate(Guid notificationId, int number);
    }
}
