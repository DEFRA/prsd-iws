namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification;

    public interface IImportNotificationsRepository
    {
        Task<IEnumerable<DataImportNotification>> GetDataImportNotificationData(DateTime from, DateTime to, UKCompetentAuthority competentAuthority);
    }
}