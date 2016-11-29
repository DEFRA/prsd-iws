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
                return new CurrentFinancialGuaranteeDetails();
            }

            var financialGuarantee =
                financialGuaranteeCollection.FinancialGuarantees.OrderByDescending(fg => fg.DecisionDate)
                    .Select(fg => new CurrentFinancialGuaranteeDetails
                    {
                        ActiveLoadsPermitted = fg.ActiveLoadsPermitted.Value,
                        Decision = fg.Status.ToString(),
                        DecisionDate = fg.DecisionDate.Value,
                        FinancialGuaranteeId = fg.Id,
                        NotificationId = financialGuaranteeCollection.NotificationId,
                        ReferenceNumber = fg.ReferenceNumber,
                        IsBlanketBond = fg.IsBlanketBond.Value
                    })
                    .First();

            return financialGuarantee;
        }
    }
}