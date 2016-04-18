namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Domain;

    internal class AuditStatusChange : IEventHandler<ImportFinancialGuaranteeStatusChangeEvent>
    {
        private readonly ImportNotificationContext context;
        private readonly IUserContext userContext;

        public AuditStatusChange(ImportNotificationContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task HandleAsync(ImportFinancialGuaranteeStatusChangeEvent @event)
        {
            @event.Guarantee.AddStatusChangeRecord(new ImportFinancialGuaranteeStatusChange(@event.Source, 
                @event.Destination, 
                userContext.UserId, 
                DateTimeOffset.UtcNow));

            await context.SaveChangesAsync();
        }
    }
}
