namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMovementRepository
    {
        Task<IEnumerable<Movement>> GetSubmittedMovements(Guid notificationId);

        Task<Movement> GetById(Guid movementId);
    }
}