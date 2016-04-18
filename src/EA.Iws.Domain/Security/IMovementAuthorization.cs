namespace EA.Iws.Domain.Security
{
    using System;
    using System.Threading.Tasks;

    public interface IMovementAuthorization
    {
        Task EnsureAccessAsync(Guid movementId);
    }
}