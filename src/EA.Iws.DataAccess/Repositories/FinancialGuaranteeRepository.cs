namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Data.SqlClient;
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

        public async Task SetCurrentFinancialGuaranteeDates(Guid notificationId, DateTime? receivedDate, DateTime? completedDate,
            DateTime? decisionDate)
        {
            await authorization.EnsureAccessAsync(notificationId);
            await context.Database.ExecuteSqlCommandAsync(@"[Notification].[uspUpdateExportFinancialGuaranteeDates] 
                @NotificationId,
                @ReceivedDate,
                @CompletedDate,
                @DecisionDate",
                new SqlParameter("@NotificationId", notificationId),
                new SqlParameter("@ReceivedDate", (object)receivedDate ?? DBNull.Value),
                new SqlParameter("@CompletedDate", (object)completedDate ?? DBNull.Value),
                new SqlParameter("@DecisionDate", (object)decisionDate ?? DBNull.Value));
        }
    }
}