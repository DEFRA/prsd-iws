namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;

    public interface IMovementDateValidator
    {
        Task EnsureDateValid(Guid notificationId, DateTime date);
    }
}