namespace EA.Iws.RequestHandlers.WasteType
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.WasteType;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    internal class GetPhysicalCharacteristicsHandler :
        IRequestHandler<GetPhysicalCharacteristics, PhysicalCharacteristicsData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, PhysicalCharacteristicsData> mapper;

        public GetPhysicalCharacteristicsHandler(IwsContext context,
            IMap<NotificationApplication, PhysicalCharacteristicsData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<PhysicalCharacteristicsData> HandleAsync(GetPhysicalCharacteristics message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            return mapper.Map(notification);
        }
    }
}