namespace EA.Iws.RequestHandlers.Movement
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class AuditMovementHandler : IRequestHandler<AuditMovement, bool>
    {
        private readonly IwsContext context;
        private readonly IMovementAuditRepository repository;
        private readonly IMapper mapper;

        public AuditMovementHandler(IwsContext context, IMovementAuditRepository repository, IMapper mapper)
        {
            this.context = context;
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<bool> HandleAsync(AuditMovement message)
        {
            await repository.Add(mapper.Map<MovementAudit>(message));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
