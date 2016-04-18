namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Domain;

    internal class FinancialGuaranteeStatusChangeEventHandler : IEventHandler<FinancialGuaranteeStatusChangeEvent>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public FinancialGuaranteeStatusChangeEventHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task HandleAsync(FinancialGuaranteeStatusChangeEvent @event)
        {
            var user = await context.Users.SingleAsync(u => u.Id == userContext.UserId.ToString());

            @event.FinancialGuarantee.AddStatusChangeRecord(new FinancialGuaranteeStatusChange(@event.TargetStatus, user));

            await context.SaveChangesAsync();
        }
    }
}
