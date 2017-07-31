namespace EA.Iws.RequestHandlers.ImportMovement.Capture
{
    using Core.ImportMovement;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;
    using Requests.ImportNotification;
    using System.Threading.Tasks;

    internal class GetImportInternalMovementSummaryHandler : IRequestHandler<GetImportInternalMovementSummary, ImportInternalMovementSummary>
    {
        private readonly IImportMovementsSummaryRepository movementSummaryRepository;
        public GetImportInternalMovementSummaryHandler(IImportMovementsSummaryRepository movementSummaryRepository)
        {
            this.movementSummaryRepository = movementSummaryRepository;
        }

        public async Task<ImportInternalMovementSummary> HandleAsync(GetImportInternalMovementSummary message)
        {
            var movementSummary = await movementSummaryRepository.GetById(message.ImportNotificationId);

            var averageData = await movementSummaryRepository.AveragePerShipment(message.ImportNotificationId);
                //Task.Run(() => mediator.SendAsync(nmovementSummaryRepository.)).Result; 
            return new ImportInternalMovementSummary
            {
                SummaryData = movementSummary,
                AverageTonnage = averageData.Quantity,
                AverageDataUnit = averageData.Units
            };
        }
    }
}
