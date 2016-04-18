namespace EA.Iws.Domain
{
    using System;
    using System.Threading.Tasks;

    public interface IInterimMovementDocumentGenerator
    {
        Task<byte[]> Generate(Guid movementId);
    }
}
