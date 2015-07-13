namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class GetEntryCustomsOfficeAddDataByNotificationIdHandler :
        IRequestHandler<GetEntryCustomsOfficeAddDataByNotificationId, EntryCustomsOfficeAddData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, EntryCustomsOfficeAddData> customsOfficeEntryMap;

        public GetEntryCustomsOfficeAddDataByNotificationIdHandler(IwsContext context,
            IMap<NotificationApplication, EntryCustomsOfficeAddData> customsOfficeEntryMap)
        {
            this.context = context;
            this.customsOfficeEntryMap = customsOfficeEntryMap;
        }

        public async Task<EntryCustomsOfficeAddData> HandleAsync(GetEntryCustomsOfficeAddDataByNotificationId message)
        {
            var notification = await context.GetNotificationApplication(message.Id);

            return customsOfficeEntryMap.Map(notification);
        }
    }
}