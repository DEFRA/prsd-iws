namespace EA.Iws.RequestHandlers.ImportNotificationMovements
{
    using System.Threading.Tasks;
    using Core.ImportNotificationMovements;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationMovements;

    internal class GetImportMovementsSummaryHandler : IRequestHandler<GetImportMovementsSummary, Summary>
    {
        private readonly IImportMovementsSummaryRepository movementSummaryRepository;

        public GetImportMovementsSummaryHandler(IImportMovementsSummaryRepository movementSummaryRepository)
        {
            this.movementSummaryRepository = movementSummaryRepository;
        }

        public async Task<Summary> HandleAsync(GetImportMovementsSummary message)
        {
            var movementSummary = await movementSummaryRepository.GetById(message.ImportNotificationId);

            return movementSummary;
        }
    }
}
