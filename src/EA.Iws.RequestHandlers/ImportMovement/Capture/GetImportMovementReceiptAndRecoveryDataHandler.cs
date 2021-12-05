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
            var result = mapper.Map<ImportMovementSummaryData>(data);
            if (data.Receipt != null)
            {
                result.IsReceived = true;
            }
            else if (data.Rejection != null)
            {
                result.RejectedQuantity = data.Rejection.RejectedQuantity;
                result.RejectedUnit = data.Rejection.RejectedUnit;
                result.IsRejected = true;
            }
            else if (data.PartialRejection != null)
            {
                result.ReceiptData.ReceiptDate = data.PartialRejection.WasteReceivedDate;
                result.ReceiptData.ActualQuantity = data.PartialRejection.ActualQuantity;
                result.ReceiptData.ReceiptUnits = data.PartialRejection.ActualUnit;
                result.ReceiptData.RejectionReason = data.PartialRejection.Reason;
                result.RejectedQuantity = data.PartialRejection.RejectedQuantity;
                result.RejectedUnit = data.PartialRejection.RejectedUnit;
                result.IsPartiallyRejected = true;
                result.ReceiptData.IsPartiallyRejected = true;
                result.RecoveryData.OperationCompleteDate = data.PartialRejection.WasteDisposedDate;
                result.RecoveryData.IsOperationCompleted = true;
            }
            return result;
        }
    }
}
