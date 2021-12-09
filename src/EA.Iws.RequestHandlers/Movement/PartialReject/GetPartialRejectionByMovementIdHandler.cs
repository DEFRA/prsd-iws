namespace EA.Iws.RequestHandlers.Movement.PartialReject
{
    using System.Threading.Tasks;
    using Domain.Movement;
    using EA.Iws.Requests.Movement.PartialReject;
    using Prsd.Core.Mediator;

    public class GetPartialRejectionByMovementIdHandler : IRequestHandler<GetPartialRejectionByMovementId, bool>
    {
        private readonly IMovementPartialRejectionRepository movementPartialRepository;

        public GetPartialRejectionByMovementIdHandler(IMovementPartialRejectionRepository movementPartialRepository)
        {
            this.movementPartialRepository = movementPartialRepository;
        }

        public async Task<bool> HandleAsync(GetPartialRejectionByMovementId message)
        {
            var partialReject = await movementPartialRepository.GetByMovementIdOrDefault(message.MovementId);
            if (partialReject == null)
            {
                return false;
            }

            return true;
        }
    }
}
