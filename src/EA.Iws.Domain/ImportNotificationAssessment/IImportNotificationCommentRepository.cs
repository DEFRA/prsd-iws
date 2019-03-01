namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IImportNotificationCommentRepository
    {
        Task<bool> Add(ImportNotificationComment comment);

        Task<List<ImportNotificationComment>> GetComments(Guid notificationId);
    }
}
