namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class GetExitCustomsOfficeAddDataByNotificationIdHandler :
        IRequestHandler<GetExitCustomsOfficeAddDataByNotificationId, ExitCustomsOfficeAddData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, ExitCustomsOfficeAddData> customsOfficeExitMap;

        public GetExitCustomsOfficeAddDataByNotificationIdHandler(IwsContext context,
            IMap<NotificationApplication, ExitCustomsOfficeAddData> customsOfficeExitMap)
        {
            this.context = context;
            this.customsOfficeExitMap = customsOfficeExitMap;
        }

        public async Task<ExitCustomsOfficeAddData> HandleAsync(GetExitCustomsOfficeAddDataByNotificationId message)
        {
            var notification = await context.GetNotificationApplication(message.Id);

            return customsOfficeExitMap.Map(notification);
        }
    }
}