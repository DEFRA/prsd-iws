namespace EA.Iws.Domain.NotificationApplication
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification;

    public interface IExportNotificationOwnerDisplayRepository
    {
        Task<IEnumerable<ExportNotificationOwnerDisplay>> GetInternalUnsubmittedByCompetentAuthority(UKCompetentAuthority competentAuthority);
    }
}