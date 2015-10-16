namespace EA.Iws.Domain
{
    using System;
    using System.Threading.Tasks;

    public interface IMovementDocumentGenerator
    {
        Task<byte[]> Generate(Guid movementId);
    }
}
