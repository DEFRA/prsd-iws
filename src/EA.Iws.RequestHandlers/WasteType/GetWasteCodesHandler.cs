namespace EA.Iws.RequestHandlers.WasteType
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    internal class GetWasteCodesHandler : IRequestHandler<GetWasteCodes, WasteCodeData[]>
    {
        private readonly IwsContext context;
        private readonly IMap<WasteCode, WasteCodeData> mapper;

        public GetWasteCodesHandler(IwsContext context, IMap<WasteCode, WasteCodeData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<WasteCodeData[]> HandleAsync(GetWasteCodes message)
        {
            var result = await context.WasteCodes.ToArrayAsync();
            return result.Select(c => mapper.Map(c)).ToArray();
        }
    }
}