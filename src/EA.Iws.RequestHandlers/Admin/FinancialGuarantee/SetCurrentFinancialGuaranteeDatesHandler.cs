namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Threading.Tasks;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class SetCurrentFinancialGuaranteeDatesHandler : IRequestHandler<SetCurrentFinancialGuaranteeDates, Unit>
    {
        private readonly IFinancialGuaranteeRepository repository;

        public SetCurrentFinancialGuaranteeDatesHandler(IFinancialGuaranteeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> HandleAsync(SetCurrentFinancialGuaranteeDates message)
        {
            await repository.SetCurrentFinancialGuaranteeDates(
                message.NotificationId, message.ReceivedDate,
                message.CompletedDate, message.DecisionDate);

            return Unit.Value;
        }
    }
}