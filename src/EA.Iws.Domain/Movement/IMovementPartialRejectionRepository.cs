﻿namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMovementPartialRejectionRepository
    {
        Task<MovementPartialRejection> GetByMovementId(Guid movementId);

        Task<MovementPartialRejection> Get(Guid id);

        Task<MovementPartialRejection> GetByMovementIdOrDefault(Guid movementId);

        void Add(MovementPartialRejection movementRejection);

        Task<IEnumerable<MovementPartialRejection>> GetMovementPartialRejectionsByMovementIds(IEnumerable<Guid> movementIds);
    }
}
