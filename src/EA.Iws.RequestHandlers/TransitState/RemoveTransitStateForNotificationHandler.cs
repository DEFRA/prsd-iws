namespace EA.Iws.RequestHandlers.TransitState
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.TransitState;

    internal class RemoveTransitStateForNotificationHandler : IRequestHandler<RemoveTransitStateForNotification, bool>
    {
        private readonly IwsContext context;

        public RemoveTransitStateForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(RemoveTransitStateForNotification message)
        {
            await context.CheckNotificationAccess(message.NotificationId);

            var transportRoute = await context.TransportRoutes.SingleAsync(p => p.NotificationId == message.NotificationId);

            transportRoute.RemoveTransitState(message.TransitStateId);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
