namespace EA.Iws.RequestHandlers.ImportMovement.Capture
{
    using System.Threading.Tasks;
    using Core.ImportMovement;
    using Domain.ImportMovement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;

    internal class GetImportMovementReceiptAndRecoveryDataHandler : IRequestHandler<GetImportMovementReceiptAndRecoveryData, ImportMovementSummaryData>
    {
        private readonly IImportMovementSummaryRepository summaryRepository;
        private readonly IMapper mapper;

        public GetImportMovementReceiptAndRecoveryDataHandler(IImportMovementSummaryRepository summaryRepository, IMapper mapper)
        {
            this.summaryRepository = summaryRepository;
            this.mapper = mapper;
        }

        public async Task<ImportMovementSummaryData> HandleAsync(GetImportMovementReceiptAndRecoveryData message)
        {
            var data = await summaryRepository.Get(message.ImportMovementId);

            return mapper.Map<ImportMovementSummaryData>(data);
        }
    }
}
