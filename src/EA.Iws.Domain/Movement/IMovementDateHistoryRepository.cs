namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMovementDateHistoryRepository
    {
        Task<IEnumerable<MovementDateHistory>> GetByMovementId(Guid movementId);
    }
}