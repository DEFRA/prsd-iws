namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class SetFinancialGuaranteeDatesHandler : IRequestHandler<SetFinancialGuaranteeDates, bool>
    {
        private readonly IFinancialGuaranteeRepository repository;
        private readonly IwsContext context;

        public SetFinancialGuaranteeDatesHandler(IFinancialGuaranteeRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetFinancialGuaranteeDates message)
        {
            var financialGuaranteeCollection = await repository.GetByNotificationId(message.NotificationId);
            var financialGuarantee = financialGuaranteeCollection.GetFinancialGuarantee(message.FinancialGuaranteeId);

            if (message.ReceivedDate.HasValue)
            {
                if (financialGuarantee.Status == FinancialGuaranteeStatus.AwaitingApplication)
                {
                    financialGuarantee.Received(message.ReceivedDate.Value);
                }
                else if (financialGuarantee.ReceivedDate != message.ReceivedDate)
                {
                    financialGuarantee.UpdateReceivedDate(message.ReceivedDate.Value);
                }
            }

            if (message.CompletedDate.HasValue && financialGuarantee.ReceivedDate.HasValue)
            {
                if (financialGuarantee.Status == FinancialGuaranteeStatus.ApplicationReceived)
                {
                    financialGuarantee.Completed(message.CompletedDate.Value);
                }
                else if (financialGuarantee.CompletedDate.HasValue
                    && financialGuarantee.CompletedDate != message.CompletedDate)
                {
                    financialGuarantee.UpdateCompletedDate(message.CompletedDate.Value);
                }
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
