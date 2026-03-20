namespace EA.Iws.DataAccess.Repositories
{
    using Domain.Finance;
    using Domain.NotificationApplication;
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class PriceRepository : IPriceRepository
    {
        private readonly IwsContext context;

        public PriceRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<PriceAndRefund> GetPriceAndRefundByNotificationId(Guid notificationId)
        {
            return await context.Database.SqlQuery<PriceAndRefund>(@"
                SELECT * FROM [Notification].[GetPricingInfo] (@NotificationId)",
                new SqlParameter("@NotificationId", notificationId)).FirstOrDefaultAsync();
        }

        public async Task<PriceAndRefund> GetDraftImportNotificationPrice(Guid notificationId)
        {
            return await context.Database.SqlQuery<PriceAndRefund>(@"SELECT * FROM [ImportNotification].[GetDraftPricing] (@NotificationId)",
                                                                   new SqlParameter("@NotificationId", notificationId))
                                         .FirstOrDefaultAsync();
        }
    }
}