namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System.Threading.Tasks;
    using Core.ImportNotification.Update;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class GetWasteOperationDataHandler : IRequestHandler<GetWasteOperationData, WasteOperationData>
    {
        private readonly IImportNotificationRepository notificationRepository;
        private readonly IWasteOperationRepository wasteOperationRepository;
        private readonly IMapper mapper;

        public GetWasteOperationDataHandler(IImportNotificationRepository notificationRepository,
            IWasteOperationRepository wasteOperationRepository, IMapper mapper)
        {
            this.notificationRepository = notificationRepository;
            this.wasteOperationRepository = wasteOperationRepository;
            this.mapper = mapper;
        }

        public async Task<WasteOperationData> HandleAsync(GetWasteOperationData message)
        {
            var details = await notificationRepository.GetDetails(message.ImportNotificationId);
            var wasteOperation = await wasteOperationRepository.GetByNotificationId(message.ImportNotificationId);

            return mapper.Map<WasteOperationData>(wasteOperation, details);
        }
    }
}
