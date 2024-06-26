﻿namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWasteComponentRepository
    {
        Task<IEnumerable<WasteComponent>> GetByNotificationId(Guid notificationId);

        void Add(List<WasteComponent> wasteComponents);
    }
}
