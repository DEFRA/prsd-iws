namespace EA.Iws.RequestHandlers.TechnologyEmployed
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.TechnologyEmployed;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.TechnologyEmployed;

    internal class GetTechnologyEmployedHandler : IRequestHandler<GetTechnologyEmployed, TechnologyEmployedData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, TechnologyEmployedData> mapper;

        public GetTechnologyEmployedHandler(IwsContext context,
            IMap<NotificationApplication, TechnologyEmployedData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TechnologyEmployedData> HandleAsync(GetTechnologyEmployed message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            return mapper.Map(notification);
        }
    }
}