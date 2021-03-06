﻿namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    public interface IRejectImportMovement
    {
        Task<ImportMovementRejection> Reject(Guid importMovementId, DateTime date, string reason);
    }
}
