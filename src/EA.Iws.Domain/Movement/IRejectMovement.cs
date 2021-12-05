namespace EA.Iws.Domain.Movement
{
    using EA.Iws.Core.Shared;
    using System;
    using System.Threading.Tasks;

    public interface IRejectMovement
    {
        Task<MovementRejection> Reject(Guid movementId,
            DateTime rejectionDate,
            string reason, 
            decimal? quantity,
            ShipmentQuantityUnits? unit);
    }
}