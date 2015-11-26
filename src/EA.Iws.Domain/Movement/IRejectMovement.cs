namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;

    public interface IRejectMovement
    {
        Task<MovementRejection> Reject(Guid movementId,
            DateTimeOffset rejectionDate,
            string reason,
            string furtherDetails = null);
    }
}