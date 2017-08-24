namespace EA.Iws.RequestHandlers.Movement
{
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using System.Threading.Tasks;

    internal class DoesMovementDetailsExistHandler : IRequestHandler<DoesMovementDetailsExist, bool>
    {
        private readonly IMovementDetailsRepository movementDetailsRepository;

        public DoesMovementDetailsExistHandler(IMovementDetailsRepository movementDetailsRepository)
        {
            this.movementDetailsRepository = movementDetailsRepository;
        }
        public async Task<bool> HandleAsync(DoesMovementDetailsExist message)
        {
            var movementDetails = await movementDetailsRepository.GetByMovementId(message.MovementId);
            if (movementDetails == null)
            {
                return false;
            }

            return true;
        }
    }
}
