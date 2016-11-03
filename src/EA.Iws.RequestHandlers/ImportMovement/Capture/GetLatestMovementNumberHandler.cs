namespace EA.Iws.RequestHandlers.ImportMovement.Capture
{
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;

    public class GetLatestMovementNumberHandler : IRequestHandler<GetLatestMovementNumber, int>
    {
        private readonly IImportMovementRepository movementRepository;

        public GetLatestMovementNumberHandler(IImportMovementRepository movementRepository)
        {
            this.movementRepository = movementRepository;
        }

        public async Task<int> HandleAsync(GetLatestMovementNumber message)
        {
            return await movementRepository.GetLatestMovementNumber(message.NotificationId);
        }
    }
}
