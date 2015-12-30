namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IImportNotificationRepository
    {
        Task<bool> NotificationNumberExists(string number);

        Task<ImportNotification> Get(Guid id);

        Task Add(ImportNotification notification);

        Task<IEnumerable<ImportNotification>> SearchByNumber(string number);
    }
}
