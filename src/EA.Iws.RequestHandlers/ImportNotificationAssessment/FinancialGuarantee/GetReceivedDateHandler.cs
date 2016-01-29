namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.FinancialGuarantee;

    internal class GetReceivedDateHandler : IRequestHandler<GetReceivedDate, DateTime?>
    {
        private readonly IImportFinancialGuaranteeRepository financialGuaranteeRepository;

        public GetReceivedDateHandler(IImportFinancialGuaranteeRepository financialGuaranteeRepository)
        {
            this.financialGuaranteeRepository = financialGuaranteeRepository;
        }

        public async Task<DateTime?> HandleAsync(GetReceivedDate message)
        {
            var financialGuarantee =
                await financialGuaranteeRepository.GetByNotificationIdOrDefault(message.ImportNotificationId);

            return financialGuarantee == null ? (DateTime?)null : financialGuarantee.ReceivedDate;
        }
    }
}
