namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class GetCurrentFinancialGuaranteeDetailsHandler :
        IRequestHandler<GetCurrentFinancialGuaranteeDetails, CurrentFinancialGuaranteeDetails>
    {
        private readonly IFinancialGuaranteeRepository repository;

        public GetCurrentFinancialGuaranteeDetailsHandler(IFinancialGuaranteeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<CurrentFinancialGuaranteeDetails> HandleAsync(GetCurrentFinancialGuaranteeDetails message)
        {
            var financialGuaranteeCollection = await repository.GetByNotificationId(message.NotificationId);

            if (!financialGuaranteeCollection.FinancialGuarantees.Any())
            {
                return new CurrentFinancialGuaranteeDetails
                {
                    NotificationId = financialGuaranteeCollection.NotificationId
                };
            }

            var financialGuarantee = financialGuaranteeCollection.GetLatestFinancialGuarantee();

            return new CurrentFinancialGuaranteeDetails
            {
                FinancialGuaranteeId = financialGuarantee.Id,
                NotificationId = financialGuaranteeCollection.NotificationId,
                ReceivedDate = financialGuarantee.ReceivedDate,
                CompletedDate = financialGuarantee.CompletedDate,
                Decision = financialGuarantee.Decision,
                Status = financialGuarantee.Status
            };
        }
    }
}