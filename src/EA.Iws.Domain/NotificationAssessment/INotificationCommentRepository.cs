namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EA.Iws.Core.Admin;

    public interface INotificationCommentRepository
    {
        Task<bool> Add(NotificationComment comment);
        Task<List<NotificationComment>> GetComments(Guid notificationId, NotificationShipmentsCommentsType type, DateTime startDate, DateTime endDate, int shipmentNumber);
        Task<List<NotificationComment>> GetPagedComments(Guid notificationId, NotificationShipmentsCommentsType type, int pageNumber, int pageSize, DateTime startDate, DateTime endDate, int shipmentNumber);

        Task<bool> Delete(Guid commentId);
        Task<int> GetTotalNumberOfComments(Guid notificationId, NotificationShipmentsCommentsType type);
    }
}
