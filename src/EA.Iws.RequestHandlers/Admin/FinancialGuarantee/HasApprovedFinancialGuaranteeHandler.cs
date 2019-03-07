namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Threading.Tasks;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class HasApprovedFinancialGuaranteeHandler : IRequestHandler<HasApprovedFinancialGuarantee, bool>
    {
        private readonly IFinancialGuaranteeRepository repository;

        public HasApprovedFinancialGuaranteeHandler(IFinancialGuaranteeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(HasApprovedFinancialGuarantee message)
        {
            var financialGuaranteeCollection = await repository.GetByNotificationId(message.NotificationId);
            return (financialGuaranteeCollection.GetCurrentApprovedFinancialGuarantee() != null);
        }
    }
}
