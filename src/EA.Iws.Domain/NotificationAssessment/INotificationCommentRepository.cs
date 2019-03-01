namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INotificationCommentRepository
    {
        Task<bool> Add(NotificationComment comment);
        Task<List<NotificationComment>> GetComments(Guid notificationId);
        Task<bool> Delete(Guid commentId);
    }
}
