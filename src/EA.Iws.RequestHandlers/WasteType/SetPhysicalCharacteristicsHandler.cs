namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    internal class SetPhysicalCharacteristicsHandler : IRequestHandler<SetPhysicalCharacteristics, Guid>
    {
        private readonly IwsContext context;
        private readonly IMap<SetPhysicalCharacteristics, IList<PhysicalCharacteristicsInfo>> mapper;

        public SetPhysicalCharacteristicsHandler(IwsContext context,
            IMap<SetPhysicalCharacteristics, IList<PhysicalCharacteristicsInfo>> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Guid> HandleAsync(SetPhysicalCharacteristics command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);
            var physicalCharacteristics = mapper.Map(command);

            notification.SetPhysicalCharacteristics(physicalCharacteristics);

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}