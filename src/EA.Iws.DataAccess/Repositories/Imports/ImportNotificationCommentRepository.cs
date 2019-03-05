namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment;
    using EA.Iws.Core.Admin;

    internal class ImportNotificationCommentRepository : IImportNotificationCommentRepository
    {
        private readonly ImportNotificationContext context;
        public ImportNotificationCommentRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<bool> Add(ImportNotificationComment comment)
        {
            context.ImportNotificationComments.Add(comment);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid commentId)
        {
            var comment = await context.ImportNotificationComments.FirstOrDefaultAsync(p => p.Id == commentId);

            if (comment == null)
            {
                return false;
            }
            context.ImportNotificationComments.Remove(comment);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ImportNotificationComment>> GetComments(Guid notificationId, DateTime startDate, DateTime endDate, int shipmentNumber)
        {
            DateTime endDateForQuery = endDate == DateTime.MaxValue ? endDate : endDate.AddDays(1);

            if (shipmentNumber == default(int))
            {
                return await context.ImportNotificationComments.Where(p => p.NotificationId == notificationId && p.DateAdded >= startDate && p.DateAdded < endDateForQuery).ToListAsync();
            }

            return await context.ImportNotificationComments.Where(p => p.NotificationId == notificationId && p.DateAdded >= startDate && p.DateAdded < endDateForQuery && p.ShipmentNumber == shipmentNumber).ToListAsync();
        }

        public async Task<int> GetTotalNumberOfComments(Guid notificationId, NotificationShipmentsCommentsType type)
        {
            if (type == NotificationShipmentsCommentsType.Shipments)
            {
                return await context.ImportNotificationComments.CountAsync(p => p.NotificationId == notificationId & p.ShipmentNumber > 0);
            }

            return await context.ImportNotificationComments.CountAsync(p => p.NotificationId == notificationId && p.ShipmentNumber == 0);
        }
    }
}
