namespace EA.Iws.RequestHandlers.WasteRecovery
{
    using System.Threading.Tasks;
    using Domain.NotificationApplication.WasteRecovery;
    using Prsd.Core.Mediator;
    using Requests.WasteRecovery;

    internal class GetDisposalMethodHandler : IRequestHandler<GetDisposalMethod, string>
    {
        private readonly IWasteDisposalRepository wasteDisposalRepository;

        public GetDisposalMethodHandler(IWasteDisposalRepository wasteDisposalRepository)
        {
            this.wasteDisposalRepository = wasteDisposalRepository;
        }

        public async Task<string> HandleAsync(GetDisposalMethod message)
        {
            var disposalInfo = await wasteDisposalRepository.GetByNotificationId(message.NotificationId);

            return disposalInfo == null ? string.Empty : disposalInfo.Method ?? string.Empty;
        }
    }
}
