namespace EA.Iws.RequestHandlers.ImportMovement
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportMovement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement;

    internal class AuditImportMovementHandler : IRequestHandler<AuditImportMovement, bool>
    {
        private readonly IImportMovementAuditRepository repository;
        private readonly IMapper mapper;
        private readonly ImportNotificationContext context;

        public AuditImportMovementHandler(IImportMovementAuditRepository repository, 
            IMapper mapper,
            ImportNotificationContext context)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<bool> HandleAsync(AuditImportMovement message)
        {
            await repository.Add(mapper.Map<ImportMovementAudit>(message));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
