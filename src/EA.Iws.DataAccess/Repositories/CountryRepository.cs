namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
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
    }
}
