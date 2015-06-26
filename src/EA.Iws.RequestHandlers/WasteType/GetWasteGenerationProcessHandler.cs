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

    internal class GetWasteGenerationProcessHandler :
        IRequestHandler<GetWasteGenerationProcess, WasteGenerationProcessData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, WasteGenerationProcessData> mapper;

        public GetWasteGenerationProcessHandler(IwsContext context,
            IMap<NotificationApplication, WasteGenerationProcessData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<WasteGenerationProcessData> HandleAsync(GetWasteGenerationProcess message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return mapper.Map(notification);
        }
    }
}