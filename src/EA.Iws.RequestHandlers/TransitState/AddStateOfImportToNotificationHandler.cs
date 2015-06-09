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

    internal class AddTransitStateToNotificationHandler : IRequestHandler<AddTransitStateToNotification, Guid>
    {
        private readonly IwsContext context;

        public AddTransitStateToNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(AddTransitStateToNotification message)
        {
            var notification = await context.NotificationApplications.SingleAsync(na => na.Id == message.NotificationId);

            var country = await context.Countries.SingleAsync(c => c.Id == message.CountryId);
            var competentAuthority =
                await context.CompetentAuthorities.SingleAsync(ca => ca.Id == message.CompetentAuthorityId);
            var entryPoint = await context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.EntryPointId);
            var exitPoint = await context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.ExitPointId);

            var ordinalPosition = notification.GetAvailableTransitStatePositions().First();
            var transitState = new TransitState(country, competentAuthority, entryPoint, exitPoint, ordinalPosition);
            
            notification.AddTransitStateToNotification(transitState);

            await context.SaveChangesAsync();

            return transitState.Id;
        }
    }
}
