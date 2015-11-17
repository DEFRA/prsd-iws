namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class WasteCodeRepository : IWasteCodeRepository
    {
        private readonly IwsContext context;

        public WasteCodeRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<WasteCode>> GetAllWasteCodes()
        {
            return await context.WasteCodes.ToArrayAsync();
        }

        public async Task<IEnumerable<WasteCode>> GetWasteCodesByIds(IEnumerable<Guid> ids)
        {
            return await context.WasteCodes.Where(wc => ids.Contains(wc.Id)).ToArrayAsync();
        }
    }
}