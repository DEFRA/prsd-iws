namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetIsPreconsentedRecoveryFacilityHandler : IRequestHandler<GetIsPreconsentedRecoveryFacility, PreconsentedFacilityData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, PreconsentedFacilityData> mapper;

        public GetIsPreconsentedRecoveryFacilityHandler(IwsContext context, IMap<NotificationApplication, PreconsentedFacilityData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<PreconsentedFacilityData> HandleAsync(GetIsPreconsentedRecoveryFacility message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return mapper.Map(notification);
        }
    }
}