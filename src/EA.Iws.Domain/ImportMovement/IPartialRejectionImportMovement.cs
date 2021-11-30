namespace EA.Iws.Domain.ImportMovement
{
    using EA.Iws.Core.Shared;
    using System;
    using System.Threading.Tasks;

    public interface IPartialRejectionImportMovement
    {
        Task<ImportMovementPartialRejection> PartailReject(Guid movementId,
                                                     DateTime rejectionDate,
                                                     string reason,
                                                     decimal actualQuantity,
                                                     ShipmentQuantityUnits actualUnit,
                                                     decimal rejectedQuantity,
                                                     ShipmentQuantityUnits rejectedUnit,
                                                     DateTime? wasteDisposedDate);
    }
}
