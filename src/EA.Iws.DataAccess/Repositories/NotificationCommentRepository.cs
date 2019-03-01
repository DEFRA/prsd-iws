namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;

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

        public async Task<List<NotificationComment>> GetComments(Guid notificationId)
        {
            return await context.NotificationComments.Where(p => p.NotificationId == notificationId).ToListAsync();
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
    }
}
