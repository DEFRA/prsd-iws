namespace EA.Iws.RequestHandlers.Movement.Reject
{
    using System.Threading.Tasks;
    using Domain.Movement;
    using EA.Iws.Requests.Movement.PartialReject;
    using EA.Iws.Requests.Movement.Reject;
    using Prsd.Core.Mediator;

    public class GetRejectionByMovementIdHandler : IRequestHandler<GetRejectionByMovementId, bool>
    {
        private readonly IMovementRejectionRepository movementRejectionRepository;

        public GetRejectionByMovementIdHandler(IMovementRejectionRepository movementRejectionRepository)
        {
            this.movementRejectionRepository = movementRejectionRepository;
        }

        public async Task<bool> HandleAsync(GetRejectionByMovementId message)
        {
            var partialReject = await movementRejectionRepository.GetByMovementIdOrDefault(message.MovementId);
            if (partialReject == null)
            {
                return false;
            }

            return true;
        }
    }
}
