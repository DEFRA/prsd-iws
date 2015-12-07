namespace EA.Iws.RequestHandlers.ImportMovement.Capture
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;

    internal class CreateImportMovementHandler : IRequestHandler<CreateImportMovement, Guid>
    {
        private readonly IImportMovementRepository movementRepository;
        private readonly IwsContext context;

        public CreateImportMovementHandler(IImportMovementRepository movementRepository, 
            IwsContext context)
        {
            this.movementRepository = movementRepository;
            this.context = context;
        }

        public Task<Guid> HandleAsync(CreateImportMovement message)
        {
            throw new NotImplementedException();
        }
    }
}
