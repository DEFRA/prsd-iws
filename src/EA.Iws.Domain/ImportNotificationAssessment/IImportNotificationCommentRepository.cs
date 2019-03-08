namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EA.Iws.Core.Admin;

    public interface IImportNotificationCommentRepository
    {
        Task<bool> Add(ImportNotificationComment comment);

        Task<List<ImportNotificationComment>> GetComments(Guid notificationId, NotificationShipmentsCommentsType type, DateTime startDate, DateTime endDate, int shipmentNumber);
        Task<List<ImportNotificationComment>> GetPagedComments(Guid notificationId, NotificationShipmentsCommentsType type, int pageNumber, int pageSize, DateTime startDate, DateTime endDate, int shipmentNumber);

        Task<bool> Delete(Guid commentId);

        Task<int> GetTotalNumberOfComments(Guid notificationId, NotificationShipmentsCommentsType type);
    }
}
