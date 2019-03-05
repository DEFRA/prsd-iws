namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using EA.Iws.Core.Admin;

    internal class NotificationCommentRepository : INotificationCommentRepository
    {
        private readonly IwsContext context;

        public NotificationCommentRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> Add(NotificationComment comment)
        {
            context.NotificationComments.Add(comment);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<NotificationComment>> GetComments(Guid notificationId, DateTime startDate, DateTime endDate, int shipmentNumber)
        {
            DateTime endDateForQuery = endDate == DateTime.MaxValue ? endDate : endDate.AddDays(1);

            if (shipmentNumber == default(int))
            {
                return await context.NotificationComments.Where(p => p.NotificationId == notificationId && p.DateAdded >= startDate && p.DateAdded < endDateForQuery).ToListAsync();
            }

            return await context.NotificationComments.Where(p => p.NotificationId == notificationId && p.DateAdded >= startDate && p.DateAdded < endDateForQuery && p.ShipmentNumber == shipmentNumber).ToListAsync();
        }

        public async Task<bool> Delete(Guid commentId)
        {
            var comment = await context.NotificationComments.SingleOrDefaultAsync(p => p.Id == commentId);

            if (comment == null)
            {
                return false;
            }
            context.NotificationComments.Remove(comment);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetTotalNumberOfComments(Guid notificationId, NotificationShipmentsCommentsType type)
        {
            if (type == NotificationShipmentsCommentsType.Shipments)
            {
                return await context.NotificationComments.CountAsync(p => p.NotificationId == notificationId & p.ShipmentNumber > 0);
            }

            return await context.NotificationComments.CountAsync(p => p.NotificationId == notificationId && p.ShipmentNumber == 0);
        }
    }
}
