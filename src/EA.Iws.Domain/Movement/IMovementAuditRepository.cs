namespace EA.Iws.Domain.Movement
{
    using System.Threading.Tasks;

    public interface IMovementAuditRepository
    {
        Task Add(MovementAudit audit);
    }
}
