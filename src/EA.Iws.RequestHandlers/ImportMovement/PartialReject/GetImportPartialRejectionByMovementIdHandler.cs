namespace EA.Iws.RequestHandlers.ImportMovement.PartialReject
{
    using System.Threading.Tasks;
    using EA.Iws.Domain.ImportMovement;
    using EA.Iws.Requests.ImportMovement.PartialReject;
    using Prsd.Core.Mediator;

    internal class GetImportPartialRejectionByMovementIdHandler : IRequestHandler<GetImportPartialRejectionByMovementId, bool>
    {
        private readonly IImportMovementPartailRejectionRepository movementPartailRejectionRepository;

        public GetImportPartialRejectionByMovementIdHandler(IImportMovementPartailRejectionRepository movementPartailRejectionRepository)
        {
            this.movementPartailRejectionRepository = movementPartailRejectionRepository;
        }

        public async Task<bool> HandleAsync(GetImportPartialRejectionByMovementId message)
        {
            var movementPartial = await movementPartailRejectionRepository.GetByMovementIdOrDefault(message.MovementId);
            if (movementPartial == null)
            {
                return false;
            }

            return true;
        }
    }
}
