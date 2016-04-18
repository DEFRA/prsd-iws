namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    public class GetFinancialGuaranteeDecisionRequiredHandler : IRequestHandler<GetFinancialGuaranteeDecisionRequired, bool>
    {
        private readonly FinancialGuaranteeDecisionRequired service;

        public GetFinancialGuaranteeDecisionRequiredHandler(FinancialGuaranteeDecisionRequired service)
        {
            this.service = service;
        }

        public async Task<bool> HandleAsync(GetFinancialGuaranteeDecisionRequired message)
        {
            return await service.Calculate(message.NotificationId);
        }
    }
}