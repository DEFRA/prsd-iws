namespace EA.Iws.DataAccess.Repositories
{
    using Core.Notification;
    using Core.Shared;
    using Domain.Finance;
    using Domain.NotificationApplication;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
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
                SELECT * FROM [Utility].[GetPricingInfo] (@NotificationId)",
                new SqlParameter("@NotificationId", notificationId)).FirstOrDefaultAsync();
        }
    }
}