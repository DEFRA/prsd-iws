namespace EA.Iws.RequestHandlers.WasteComponentType
{
    using EA.Iws.DataAccess;
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.Requests.WasteComponentType;
    using EA.Prsd.Core.Mapper;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class SetWasteComponentInfoForNotificationHandler : IRequestHandler<SetWasteComponentInfoForNotification, Guid>
    {
        private readonly IwsContext context;
        private readonly IMap<SetWasteComponentInfoForNotification, IEnumerable<WasteComponentInfo>> wasteComponentInfoMapper;

        public SetWasteComponentInfoForNotificationHandler(IwsContext context, IMap<SetWasteComponentInfoForNotification, IEnumerable<WasteComponentInfo>> wasteComponentInfoMapper)
        {
            this.context = context;
            this.wasteComponentInfoMapper = wasteComponentInfoMapper;
        }

        public async Task<Guid> HandleAsync(SetWasteComponentInfoForNotification command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);

            notification.SetWasteComponentInfo(wasteComponentInfoMapper.Map(command));

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}
