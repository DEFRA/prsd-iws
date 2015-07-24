namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Notification;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetRecoveryPercentageDataHandler : IRequestHandler<GetRecoveryPercentageData, RecoveryPercentageData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, RecoveryPercentageData> recoveryPercentageMapper; 

        public GetRecoveryPercentageDataHandler(IwsContext context, IMap<NotificationApplication, RecoveryPercentageData> recoveryPercentageMapper)
        {
            this.context = context;
            this.recoveryPercentageMapper = recoveryPercentageMapper;
        }

        public async Task<RecoveryPercentageData> HandleAsync(GetRecoveryPercentageData message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            return recoveryPercentageMapper.Map(notification);
        }
    }
}
