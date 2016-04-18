namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;

    public interface IMovementRejectionRepository
    {
        Task<MovementRejection> GetByMovementId(Guid movementId);

        Task<MovementRejection> Get(Guid id);

        Task<MovementRejection> GetByMovementIdOrDefault(Guid movementId); 

        void Add(MovementRejection movementRejection);
    }
}
