namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;

    public static class IwsContextExtensions
    {
        public static async Task<IList<Movement>> GetMovementsForNotificationAsync(this IwsContext context, Guid notificationId)
        {
            return await context
                .Movements
                .Where(m => 
                    m.NotificationApplicationId == notificationId)
                .ToArrayAsync();
        }
    }
}
