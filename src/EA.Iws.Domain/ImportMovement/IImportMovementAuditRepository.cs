namespace EA.Iws.Domain.ImportMovement
{
    using System.Threading.Tasks;

    public interface IImportMovementAuditRepository
    {
        Task Add(ImportMovementAudit audit);
    }
}
