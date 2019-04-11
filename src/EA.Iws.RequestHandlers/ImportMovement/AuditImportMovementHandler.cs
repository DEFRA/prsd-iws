namespace EA.Iws.RequestHandlers.ImportMovement
{
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement;

    internal class AuditImportMovementHandler : IRequestHandler<AuditImportMovement, bool>
    {
        private readonly IImportMovementAuditRepository repository;
        private readonly IMapper mapper;

        public AuditImportMovementHandler(IImportMovementAuditRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<bool> HandleAsync(AuditImportMovement message)
        {
            await repository.Add(mapper.Map<ImportMovementAudit>(message));

            return true;
        }
    }
}
