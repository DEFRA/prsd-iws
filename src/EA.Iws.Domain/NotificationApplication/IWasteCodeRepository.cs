namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWasteCodeRepository
    {
        Task<IEnumerable<WasteCode>> GetAllWasteCodes();

        Task<IEnumerable<WasteCode>> GetWasteCodesByIds(IEnumerable<Guid> ids);
    }
}