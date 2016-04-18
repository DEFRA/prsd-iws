namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;

    internal class FinancialGuaranteeRepository : IFinancialGuaranteeRepository
    {
        private readonly IwsContext context;

        public FinancialGuaranteeRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<FinancialGuarantee> GetByNotificationId(Guid notificationId)
        {
            return await context.FinancialGuarantees.SingleAsync(fg => fg.NotificationApplicationId == notificationId);
        }

        public async Task<FinancialGuaranteeStatus> GetStatusByNotificationId(Guid notificationId)
        {
            return (await GetByNotificationId(notificationId)).Status;
        }
    }
}