namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
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

        public async Task<FinancialGuarantee> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.FinancialGuarantees.SingleAsync(fg => fg.NotificationApplicationId == notificationId);
        }

        public async Task<FinancialGuaranteeStatus> GetStatusByNotificationId(Guid notificationId)
        {
            return (await GetByNotificationId(notificationId)).Status;
        }
    }
}