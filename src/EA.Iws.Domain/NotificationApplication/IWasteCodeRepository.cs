namespace EA.Iws.Domain.NotificationApplication
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWasteCodeRepository
    {
        Task<IEnumerable<WasteCode>> GetAllWasteCodes();
    }
}