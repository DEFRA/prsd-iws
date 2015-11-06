namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using Prsd.Core;

    internal class CountryRepository : ICountryRepository
    {
        private readonly IwsContext context;

        public CountryRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Country> GetById(Guid id)
        {
            return await context.Countries.SingleAsync(c => c.Id == id);
        }

        public async Task<Country> GetByName(string name)
        {
            Guard.ArgumentNotNullOrEmpty(() => name, name);

            return
                await
                    context.Countries.SingleAsync(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<Guid> GetUnitedKingdomId()
        {
            return await context.Countries.Where(c => c.IsoAlpha2Code == "GB").Select(c => c.Id).SingleAsync();
        }
    }
}
