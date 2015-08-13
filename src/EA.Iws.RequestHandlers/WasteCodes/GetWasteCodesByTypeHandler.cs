namespace EA.Iws.RequestHandlers.WasteCodes
{
    using System.Collections.Generic;
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
            IList<WasteCode> result;

            if (message.CodeTypes == null || message.CodeTypes.Length == 0)
            {
                result = await context.WasteCodes.ToArrayAsync();
            }
            else
            {
                result = await context.WasteCodes.Where(p => message.CodeTypes.Contains(p.CodeType)).ToArrayAsync();
            }
            
            return result.Select(c => mapper.Map(c)).OrderBy(m => m.Code).ToArray();
        }
    }
}