namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class CreateFinancialGuaranteeHandler : IRequestHandler<CreateFinancialGuarantee, Guid>
    {
        private readonly IwsContext context;
        private readonly IFinancialGuaranteeRepository repository;

        public CreateFinancialGuaranteeHandler(IFinancialGuaranteeRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Guid> HandleAsync(CreateFinancialGuarantee message)
        {
            var financialGuaranteeCollection = await repository.GetByNotificationId(message.NotificationId);

            var financialGuarantee = financialGuaranteeCollection.AddFinancialGuarantee(message.ReceivedDate);

            await context.SaveChangesAsync();

            return financialGuarantee.Id;
        }
    }
}