namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.FinancialGuarantee
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment.FinancialGuarantee;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.FinancialGuarantee;

    internal class GetAvailableFinancialGuaranteeDecisionsHandler : IRequestHandler<GetAvailableFinancialGuaranteeDecisions, AvailableDecisionsData>
    {
        private readonly IImportFinancialGuaranteeRepository financialGuaranteeRepository;

        public GetAvailableFinancialGuaranteeDecisionsHandler(IImportFinancialGuaranteeRepository financialGuaranteeRepository)
        {
            this.financialGuaranteeRepository = financialGuaranteeRepository;
        }

        public async Task<AvailableDecisionsData> HandleAsync(GetAvailableFinancialGuaranteeDecisions message)
        {
            var financialGuarantee =
                await financialGuaranteeRepository.GetByNotificationIdOrDefault(message.ImportNotificationId);

            if (financialGuarantee == null)
            {
                return new AvailableDecisionsData();
            }

            return new AvailableDecisionsData
            {
                IsCompleted = financialGuarantee.CompletedDate.HasValue,
                IsReceived = true,
                Decisions = financialGuarantee.GetAvailableDecisions().ToList(),
                Status = financialGuarantee.Status
            };
        }
    }
}
