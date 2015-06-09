namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetIsPreconsentedRecoveryFacilityHandler : IRequestHandler<GetIsPreconsentedRecoveryFacility, bool>
    {
        private readonly IwsContext context;

        public GetIsPreconsentedRecoveryFacilityHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(GetIsPreconsentedRecoveryFacility message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return notification.IsPreconsentedRecoveryFacility ?? false;
        }
    }
}