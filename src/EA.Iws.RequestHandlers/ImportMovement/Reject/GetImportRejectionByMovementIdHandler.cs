namespace EA.Iws.RequestHandlers.ImportMovement.Reject
{
    using System.Threading.Tasks;
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Domain.ImportMovement;
    using EA.Iws.Requests.ImportMovement.Reject;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanEditImportMovements)]
    public class GetImportRejectionByMovementIdHandler : IRequestHandler<GetImportRejectionByMovementId, bool>
    {
        private readonly IImportMovementRejectionRepository movementRejectionRepository;

        public GetImportRejectionByMovementIdHandler(IImportMovementRejectionRepository movementRejectionRepository)
        {
            this.movementRejectionRepository = movementRejectionRepository;
        }

        public async Task<bool> HandleAsync(GetImportRejectionByMovementId message)
        {
            var rejectMovement = await movementRejectionRepository.GetByMovementIdOrDefault(message.MovementId);
            if (rejectMovement == null)
            {
                return false;
            }

            return true;
        }
    }
}
