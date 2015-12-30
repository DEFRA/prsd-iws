namespace EA.Iws.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.Finance;
    using Domain.NotificationApplication;

    public class PricingStructureRepository : IPricingStructureRepository
    {
        private readonly IwsContext context;

        public PricingStructureRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<PricingStructure>> Get()
        {
            return await context.PricingStructures.ToArrayAsync();
        }
    }
}
