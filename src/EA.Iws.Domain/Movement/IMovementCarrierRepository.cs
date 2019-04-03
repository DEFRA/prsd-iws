namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

public interface IMovementCarrierRepository
    {
        void Add(MovementCarrier movementCarrier);

        Task<IEnumerable<MovementCarrier>> GetCarriersByMovementId(Guid movementId);
    }
}
