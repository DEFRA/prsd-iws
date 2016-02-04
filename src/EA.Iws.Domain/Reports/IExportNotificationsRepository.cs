namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public interface IExportNotificationsRepository
    {
        Task<IEnumerable<DataExportNotification>> Get(DateTime from, DateTime to, CompetentAuthorityEnum competentAuthority);
    }
}
