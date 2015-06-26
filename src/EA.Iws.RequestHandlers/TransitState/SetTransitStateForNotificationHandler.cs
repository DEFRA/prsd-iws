namespace EA.Iws.RequestHandlers.TransitState
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.TransitState;

    internal class SetTransitStateForNotificationHandler : IRequestHandler<SetTransitStateForNotification, Guid>
    {
        private readonly IwsContext context;

        public SetTransitStateForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetTransitStateForNotification message)
        {
            var notification = await context.NotificationApplications.SingleAsync(na => na.Id == message.NotificationId);

            var country = await context.Countries.SingleAsync(c => c.Id == message.CountryId);
            var competentAuthority =
                await context.CompetentAuthorities.SingleAsync(ca => ca.Id == message.CompetentAuthorityId);
            var entryPoint = await context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.EntryPointId);
            var exitPoint = await context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.ExitPointId);

            Guid result;
            if (message.TransitStateId.HasValue)
            {
                notification.UpdateTransitStateForNotification(message.TransitStateId.Value,
                    country, 
                    competentAuthority, 
                    entryPoint, 
                    exitPoint, 
                    message.OrdinalPosition);

                result = message.TransitStateId.Value;
            }
            else
            {
                var ordinalPosition = notification.GetAvailableTransitStatePositions().First();
                var transitState = new TransitState(country, competentAuthority, entryPoint, exitPoint, ordinalPosition);
                result = transitState.Id;
                notification.AddTransitStateToNotification(transitState);
            }

            await context.SaveChangesAsync();

            return result;
        }
    }
}
