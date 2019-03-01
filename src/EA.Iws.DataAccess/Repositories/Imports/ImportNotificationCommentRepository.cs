namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment;

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

        public async Task<List<ImportNotificationComment>> GetComments(Guid notificationId)
        {
            return await context.ImportNotificationComments.Where(p => p.NotificationId == notificationId).ToListAsync();
        }
    }
}
