namespace EA.Iws.RequestHandlers.RecoveryInfo
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.RecoveryInfo;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.RecoveryInfo;

    internal class GetRecoveryValuesDataHandler : IRequestHandler<GetRecoveryValuesData, RecoveryInfoData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, RecoveryInfoData> recoveryInfoDataMap;

        public GetRecoveryValuesDataHandler(IwsContext context, IMap<NotificationApplication, RecoveryInfoData> recoveryInfoDataMap)
        {
            this.context = context;
            this.recoveryInfoDataMap = recoveryInfoDataMap;
        }

        public async Task<RecoveryInfoData> HandleAsync(GetRecoveryValuesData message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            return recoveryInfoDataMap.Map(notification);
        }
    }
}
