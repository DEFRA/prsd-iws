namespace EA.Iws.RequestHandlers.OperationCodes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.OperationCodes;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.OperationCodes;

    internal class GetOperationCodesByNotificationIdHandler : IRequestHandler<GetOperationCodesByNotificationId, IList<OperationCode>>
    {
        private readonly INotificationApplicationRepository notificationRepository;

        public GetOperationCodesByNotificationIdHandler(INotificationApplicationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        public async Task<IList<OperationCode>> HandleAsync(GetOperationCodesByNotificationId query)
        {
            var notification = await notificationRepository.GetById(query.NotificationId);

            return notification.OperationInfos.Select(o => o.OperationCode).ToList();
        }
    }
}
