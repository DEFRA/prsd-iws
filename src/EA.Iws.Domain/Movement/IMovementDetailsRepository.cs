namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;

    public interface IMovementDetailsRepository
    {
        Task<MovementDetails> GetByMovementId(Guid movementId);
    }
}