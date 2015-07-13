namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    public class CreateWasteTypeHandler : IRequestHandler<CreateWasteType, Guid>
    {
        private readonly IwsContext context;
        private readonly IMap<CreateWasteType, WasteType> wasteTypeMap;

        public CreateWasteTypeHandler(IwsContext context, IMap<CreateWasteType, WasteType> wasteTypeMap)
        {
            this.context = context;
            this.wasteTypeMap = wasteTypeMap;
        }

        public async Task<Guid> HandleAsync(CreateWasteType command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);

            notification.SetWasteType(wasteTypeMap.Map(command));

            await context.SaveChangesAsync();

            return notification.WasteType.Id;
        }
    }
}