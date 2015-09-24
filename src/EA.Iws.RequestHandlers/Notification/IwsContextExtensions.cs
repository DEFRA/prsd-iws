namespace EA.Iws.RequestHandlers.Notification
{
    using EA.Iws.DataAccess;
    using EA.Iws.Domain.NotificationApplication;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class IwsContextExtensions
    {
        public static async Task<ShipmentInfo> GetShipmentInfoAsync(this IwsContext context, Guid notificationId)
        {
            return await context
                .ShipmentInfos
                .SingleOrDefaultAsync(si => 
                    si.NotificationId == notificationId);
        }
    }
}
