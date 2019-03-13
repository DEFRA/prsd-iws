namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class CheckFinancialGuranteeStatusHandler : IRequestHandler<CheckFinancialGuaranteeStatus, bool>
    {
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;

        public CheckFinancialGuranteeStatusHandler(IFinancialGuaranteeRepository financialGuaranteeRepository)
        {
            this.financialGuaranteeRepository = financialGuaranteeRepository;
        }

        public async Task<bool> HandleAsync(CheckFinancialGuaranteeStatus message)
        {
            var financialGuaranteeCollection = await financialGuaranteeRepository.GetByNotificationId(message.NotificationId);

            if (financialGuaranteeCollection.FinancialGuarantees.Count() == 0)
            {
                return false;
            }
            else
            {
                return 
                    financialGuaranteeCollection.FinancialGuarantees.Any(
                        fg => fg.Status == FinancialGuaranteeStatus.Approved
                            || fg.Status == FinancialGuaranteeStatus.Released);
            }
        }
    }
}
