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

        public async Task<List<NotificationComment>> GetComments(Guid notificationId, NotificationShipmentsCommentsType type, DateTime startDate, DateTime endDate, int shipmentNumber)
        {
            var allCommentsForType = await this.GetCommentsByType(notificationId, type);

            DateTime endDateForQuery = endDate == DateTime.MaxValue ? endDate : endDate.AddDays(1);

            if (shipmentNumber == default(int))
            {
                return allCommentsForType.Where(p => p.DateAdded >= startDate && p.DateAdded < endDateForQuery).ToList();
            }

            return allCommentsForType.Where(p => p.DateAdded >= startDate && p.DateAdded < endDateForQuery && p.ShipmentNumber == shipmentNumber).ToList();
        }

        public async Task<List<NotificationComment>> GetPagedComments(Guid notificationId, NotificationShipmentsCommentsType type, int pageNumber, int pageSize, DateTime startDate, DateTime endDate, int shipmentNumber)
        {
            var allCommentsForType = await this.GetCommentsByType(notificationId, type);

            DateTime endDateForQuery = endDate == DateTime.MaxValue ? endDate : endDate.AddDays(1);

            var returnComments = allCommentsForType.Where(p => p.DateAdded >= startDate && p.DateAdded < endDateForQuery);
            if (shipmentNumber != default(int))
            {
                returnComments = returnComments.Where(p => p.ShipmentNumber == shipmentNumber);
            }

            return returnComments
                    .OrderByDescending(x => x.DateAdded)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
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

            private async Task<IEnumerable<NotificationComment>> GetCommentsByType(Guid notificationId, NotificationShipmentsCommentsType type)
            {
                if (type == NotificationShipmentsCommentsType.Notification)
                {
                    return await context.NotificationComments.Where(p => p.NotificationId == notificationId && p.ShipmentNumber == 0).ToListAsync();
                }
                return await context.NotificationComments.Where(p => p.NotificationId == notificationId && p.ShipmentNumber != 0).ToListAsync();
            }
        }
}
