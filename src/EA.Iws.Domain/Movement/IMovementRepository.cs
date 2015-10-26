namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement;

    public interface IMovementRepository
    {
        Task<Movement> GetById(Guid movementId);

        Task<IEnumerable<Movement>> GetMovementsByIds(Guid notificationId, IEnumerable<Guid> movementIds);

        Task<IEnumerable<Movement>> GetAllMovements(Guid notificationId);

        Task<IEnumerable<Movement>> GetMovementsByStatus(Guid notificationId, MovementStatus status);
    }
}