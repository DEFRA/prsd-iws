namespace EA.Iws.RequestHandlers.WasteCodes
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;

    internal class GetWasteCodesByTypeHandler : IRequestHandler<GetWasteCodesByType, WasteCodeData[]>
    {
        private readonly IwsContext context;
        private readonly IMap<WasteCode, WasteCodeData> mapper;

        public GetWasteCodesByTypeHandler(IwsContext context, IMap<WasteCode, WasteCodeData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<WasteCodeData[]> HandleAsync(GetWasteCodesByType message)
        {
            var result = await context.WasteCodes.Where(p => p.CodeType == message.CodeType).ToArrayAsync();
            return result.Select(c => mapper.Map(c)).OrderBy(m => m.Code).ToArray();
        }
    }
}