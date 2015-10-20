namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMovementRepository
    {
        Task<IEnumerable<Movement>> GetSubmittedMovements(Guid notificationId);

        Task<IEnumerable<Movement>> GetReceivedMovements(Guid notificationId);

        Task<IEnumerable<Movement>> GetCompletedMovements(Guid notificationId);
        
        Task<Movement> GetById(Guid movementId);

        Task<IEnumerable<Movement>> GetMovementsByIds(Guid notificationId, IEnumerable<Guid> movementIds);

        Task<IEnumerable<Movement>> GetAllMovements(Guid notificationId);
    }
}