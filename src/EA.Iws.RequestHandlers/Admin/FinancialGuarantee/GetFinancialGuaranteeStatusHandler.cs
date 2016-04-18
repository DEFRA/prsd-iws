namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetFinancialGuaranteeStatusHandler : IRequestHandler<GetFinancialGuaranteeStatus, FinancialGuaranteeStatus>
    {
        private readonly IFinancialGuaranteeRepository repository;

        public GetFinancialGuaranteeStatusHandler(IFinancialGuaranteeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<FinancialGuaranteeStatus> HandleAsync(GetFinancialGuaranteeStatus message)
        {
            return await repository.GetStatusByNotificationId(message.NotificationId);
        }
    }
}
