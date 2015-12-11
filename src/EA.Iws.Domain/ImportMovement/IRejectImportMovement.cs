namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    public interface IRejectImportMovement
    {
        Task<ImportMovementRejection> Reject(Guid importMovementId, DateTimeOffset date, string reason,
            string furtherDetails);
    }
}
