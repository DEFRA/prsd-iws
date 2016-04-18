namespace EA.Iws.RequestHandlers.Shared
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Shared;

    internal class GetCountryHandler : IRequestHandler<GetCountry, CountryData>
    {
        private readonly IwsContext context;

        public GetCountryHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<CountryData> HandleAsync(GetCountry query)
        {
            var result = await context.Countries.SingleAsync(c => c.Id == query.CountryId);
            return new CountryData { Id = result.Id, Name = result.Name };
        }
    }
}