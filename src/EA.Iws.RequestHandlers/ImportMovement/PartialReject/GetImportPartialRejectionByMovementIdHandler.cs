namespace EA.Iws.RequestHandlers.ImportMovement.PartialReject
{
    using System.Threading.Tasks;
    using EA.Iws.Domain.ImportMovement;
    using EA.Iws.Requests.ImportMovement.PartialReject;
    using Prsd.Core.Mediator;

    internal class GetImportPartialRejectionByMovementIdHandler : IRequestHandler<GetImportPartialRejectionByMovementId, bool>
    {
        private readonly IImportMovementRejectionRepository movementRejectionRepository;

        public GetImportPartialRejectionByMovementIdHandler(IImportMovementRejectionRepository movementRejectionRepository)
        {
            this.movementRejectionRepository = movementRejectionRepository;
        }

        public async Task<bool> HandleAsync(GetImportPartialRejectionByMovementId message)
        {
            var movementPartial = await movementRejectionRepository.GetByMovementIdOrDefault(message.MovementId);
            if (movementPartial == null)
            {
                return false;
            }

            return true;
        }
    }
}
