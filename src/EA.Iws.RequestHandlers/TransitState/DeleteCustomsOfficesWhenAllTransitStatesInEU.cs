namespace EA.Iws.RequestHandlers.TransitState
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Domain;

    internal class DeleteCustomsOfficesWhenAllTransitStatesInEU : IEventHandler<AllTransitStatesInEUEvent>
    {
        private readonly IwsContext context;

        public DeleteCustomsOfficesWhenAllTransitStatesInEU(IwsContext context)
        {
            this.context = context;
        }

        public async Task HandleAsync(AllTransitStatesInEUEvent @event)
        {
            if (@event.TransportRoute.EntryCustomsOffice != null)
            {
                context.DeleteOnCommit(@event.TransportRoute.EntryCustomsOffice);
            }

            if (@event.TransportRoute.ExitCustomsOffice != null)
            {
                context.DeleteOnCommit(@event.TransportRoute.ExitCustomsOffice);
            }

            await context.SaveChangesAsync();
        }
    }
}