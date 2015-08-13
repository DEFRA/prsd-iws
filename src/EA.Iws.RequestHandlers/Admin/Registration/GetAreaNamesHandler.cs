namespace EA.Iws.RequestHandlers.Admin.Registration
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Admin;

    internal class GetAreaNamesHandler : IRequestHandler<GetAreaNames, List<string>>
    {
        private readonly IwsContext context;

        public GetAreaNamesHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<List<string>> HandleAsync(GetAreaNames query)
        {
            var result = await context.LocalAreas.OrderBy(x => x.Name).ToArrayAsync();
            return result.Select(c => c.Name).ToList();
        }
    }
}