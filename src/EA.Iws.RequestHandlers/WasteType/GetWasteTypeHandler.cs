namespace EA.Iws.RequestHandlers.WasteType
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.WasteType;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    internal class GetWasteTypeHandler : IRequestHandler<GetWasteType, WasteTypeData>
    {
        private readonly IwsContext context;
        private readonly IMap<WasteType, WasteTypeData> mapper;

        public GetWasteTypeHandler(IwsContext context, IMap<WasteType, WasteTypeData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<WasteTypeData> HandleAsync(GetWasteType message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);
            if (notification.WasteType == null)
            {
                return null;
            }
            return mapper.Map(notification.WasteType);
        }
    }
}