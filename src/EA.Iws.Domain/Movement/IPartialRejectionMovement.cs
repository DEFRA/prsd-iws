namespace EA.Iws.Domain.Movement
{
    using EA.Iws.Core.Shared;
    using System;
    using System.Threading.Tasks;

    public interface IPartialRejectionMovement
    {
        Task<MovementPartialRejection> PartailReject(Guid movementId,
                                                     DateTime rejectionDate,
                                                     string reason,
                                                     decimal actualQuantity,
                                                     ShipmentQuantityUnits actualUnit,
                                                     decimal rejectedQuantity,
                                                     ShipmentQuantityUnits rejectedUnit,
                                                     DateTime? wasteDisposedDate);
    }
}
