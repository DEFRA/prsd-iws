namespace EA.Iws.Domain.Security
{
    using System;
    using System.Threading.Tasks;

    public interface IImportMovementAuthorization
    {
        Task EnsureAccessAsync(Guid movementId);
    }
}