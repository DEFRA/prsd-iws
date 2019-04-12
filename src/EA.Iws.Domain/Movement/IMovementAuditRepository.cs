namespace EA.Iws.Domain.Movement
{
    using System.Threading.Tasks;
    using Core.Movement;

    public interface IMovementAuditRepository
    {
        Task Add(MovementAudit audit);

        Task Add(Movement movement, MovementAuditType type);
    }
}
