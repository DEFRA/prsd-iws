namespace EA.Iws.RequestHandlers.Movement
{
    using System.Threading.Tasks;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class AuditMovementHandler : IRequestHandler<AuditMovement, bool>
    {
        private readonly IMovementAuditRepository repository;
        private readonly IMapper mapper;

        public AuditMovementHandler(IMovementAuditRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<bool> HandleAsync(AuditMovement message)
        {
            await repository.Add(mapper.Map<MovementAudit>(message));

            return true;
        }
    }
}
