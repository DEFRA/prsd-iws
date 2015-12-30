namespace EA.Iws.Domain.NotificationApplication
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Finance;

    public interface IPricingStructureRepository
    {
        Task<IEnumerable<PricingStructure>> Get();
    }
}
