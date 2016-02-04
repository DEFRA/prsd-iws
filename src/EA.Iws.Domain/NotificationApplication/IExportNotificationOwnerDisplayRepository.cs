namespace EA.Iws.Domain.NotificationApplication
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public interface IExportNotificationOwnerDisplayRepository
    {
        Task<IEnumerable<ExportNotificationOwnerDisplay>> GetInternalUnsubmittedByCompetentAuthority(CompetentAuthorityEnum competentAuthority);
    }
}