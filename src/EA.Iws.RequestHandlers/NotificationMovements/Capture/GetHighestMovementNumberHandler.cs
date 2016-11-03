namespace EA.Iws.RequestHandlers.NotificationMovements.Capture
{
    using System.Threading.Tasks;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Capture;

    public class GetLatestMovementNumberHandler : IRequestHandler<GetLatestMovementNumber, int>
    {
        private readonly IMovementRepository movementRepository;

        public GetLatestMovementNumberHandler(IMovementRepository movementRepository)
        {
            this.movementRepository = movementRepository;
        }

        public async Task<int> HandleAsync(GetLatestMovementNumber message)
        {
            return await movementRepository.GetLatestMovementNumber(message.NotificationId);
        }
    }
}
