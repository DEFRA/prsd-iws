namespace EA.Iws.RequestHandlers.ImportNotification.WasteType
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification.WasteType;

    internal class GetAllWasteCodesHandler : IRequestHandler<GetAllWasteCodes, IList<WasteCodeData>>
    {
        private readonly IMapper mapper;
        private readonly IWasteCodeRepository repository;

        public GetAllWasteCodesHandler(IWasteCodeRepository repository,
            IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IList<WasteCodeData>> HandleAsync(GetAllWasteCodes message)
        {
            var result = await repository.GetAllWasteCodes();

            return result
                .Select(wasteCode => mapper.Map<WasteCodeData>(wasteCode))
                .ToList();
        }
    }
}