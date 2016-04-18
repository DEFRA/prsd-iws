namespace EA.Iws.RequestHandlers.Admin.Registration
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Admin;

    internal class GetLocalAreasHandler : IRequestHandler<GetLocalAreas, List<LocalAreaData>>
    {
        private readonly IwsContext context;

        public GetLocalAreasHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<List<LocalAreaData>> HandleAsync(GetLocalAreas query)
        {
            return await context.LocalAreas.OrderBy(x => x.Name)
                .Select(p => new LocalAreaData() { Id = p.Id, Name = p.Name, CompetentAuthorityId = p.CompetentAuthorityId})
                .ToListAsync();
        }
    }
}