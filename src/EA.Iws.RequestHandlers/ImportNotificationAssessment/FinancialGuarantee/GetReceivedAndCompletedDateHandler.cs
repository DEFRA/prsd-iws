namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.FinancialGuarantee
{
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment.FinancialGuarantee;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.FinancialGuarantee;

    internal class GetReceivedAndCompletedDateHandler : IRequestHandler<GetReceivedAndCompletedDate, ReceivedAndCompletedDateData>
    {
        private readonly IImportFinancialGuaranteeRepository financialGuaranteeRepository;

        public GetReceivedAndCompletedDateHandler(IImportFinancialGuaranteeRepository financialGuaranteeRepository)
        {
            this.financialGuaranteeRepository = financialGuaranteeRepository;
        }

        public async Task<ReceivedAndCompletedDateData> HandleAsync(GetReceivedAndCompletedDate message)
        {
            var guarantee =
                await financialGuaranteeRepository.GetByNotificationIdOrDefault(message.ImportNotificationId);

            if (guarantee == null)
            {
                return new ReceivedAndCompletedDateData();
            }

            return new ReceivedAndCompletedDateData
            {
                ReceivedDate = guarantee.ReceivedDate,
                CompletedDate = guarantee.CompletedDate
            };
        }
    }
}
