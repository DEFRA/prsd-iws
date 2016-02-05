namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification;

    public interface IExportNotificationsRepository
    {
        Task<IEnumerable<DataExportNotification>> Get(DateTime from, DateTime to, UKCompetentAuthority competentAuthority);
    }
}
