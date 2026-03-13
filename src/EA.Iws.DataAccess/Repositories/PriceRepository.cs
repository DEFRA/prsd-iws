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

        public async Task<PriceAndRefund> GetPriceAndRefundByNotificationId(Guid notificationId, DateTime? chargeDate = null)
        {
            DateTimeOffset? chargeDateTimeOffset = chargeDate.HasValue
                ? new DateTimeOffset(chargeDate.Value)
                : (DateTimeOffset?)null;

            return await context.Database.SqlQuery<PriceAndRefund>(@"
                SELECT * FROM [Notification].[GetPricingInfo] (@NotificationId, @chargeDate)",
                new SqlParameter("@NotificationId", notificationId),
                new SqlParameter("@ChargeDate", (object)chargeDateTimeOffset ?? DBNull.Value)).FirstOrDefaultAsync();
        }
    }
}