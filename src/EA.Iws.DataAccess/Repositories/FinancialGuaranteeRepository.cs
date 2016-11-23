namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.FinancialGuarantee;
    using Domain.Security;

    internal class FinancialGuaranteeRepository : IFinancialGuaranteeRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization authorization;

        public FinancialGuaranteeRepository(IwsContext context, INotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<FinancialGuaranteeCollection> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.FinancialGuarantees.SingleAsync(fg => fg.NotificationId == notificationId);
        }
    }
}