namespace EA.Iws.RequestHandlers.NotificationMovements.Capture
{
    using System.Threading.Tasks;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Capture;

    public class GetHighestMovementNumberHandler : IRequestHandler<GetHighestMovementNumber, int>
    {
        private readonly IMovementRepository movementRepository;

        public GetHighestMovementNumberHandler(IMovementRepository movementRepository)
        {
            this.movementRepository = movementRepository;
        }

        public async Task<int> HandleAsync(GetHighestMovementNumber message)
        {
            return await movementRepository.GetHighestMovementNumber(message.NotificationId);
        }
    }
}
