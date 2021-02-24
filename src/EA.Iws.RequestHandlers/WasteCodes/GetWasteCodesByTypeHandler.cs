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
        private readonly IWasteCodeRepository wasteCodeRepository;
        private readonly IMap<WasteCode, WasteCodeData> mapper;

        public GetWasteCodesByTypeHandler(IWasteCodeRepository wasteCodeRepository, IMap<WasteCode, WasteCodeData> mapper)
        {
            this.wasteCodeRepository = wasteCodeRepository;
            this.mapper = mapper;
        }

        public async Task<WasteCodeData[]> HandleAsync(GetWasteCodesByType message)
        {
            IEnumerable<WasteCode> result;

            if (message.CodeTypes == null || message.CodeTypes.Length == 0)
            {
                result = await wasteCodeRepository.GetAllWasteCodes();
            }
            else
            {
                result = await wasteCodeRepository.GetAllWasteCodes();
                result = result
                        .Where(p => message.CodeTypes.Contains(p.CodeType))
                        .ToArray();
            }
            
            return result.Select(c => mapper.Map(c)).OrderBy(m => m.Code).ToArray();
        }
    }
}