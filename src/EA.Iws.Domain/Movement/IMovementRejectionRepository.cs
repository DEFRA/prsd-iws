namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;

    public interface IMovementRejectionRepository
    {
        Task<MovementRejection> GetByMovementId(Guid movementId);

        Task<MovementRejection> Get(Guid id);

        void Add(MovementRejection movementRejection);
    }
}
