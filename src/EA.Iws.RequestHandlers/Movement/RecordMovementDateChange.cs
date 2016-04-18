namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Domain;

    internal class RecordMovementDateChange : IEventHandler<MovementDateChangeEvent>
    {
        private readonly IwsContext context;

        public RecordMovementDateChange(IwsContext context)
        {
            this.context = context;
        }

        public async Task HandleAsync(MovementDateChangeEvent @event)
        {
            var movementDateHistory = new MovementDateHistory(@event.MovementId, new DateTimeOffset(@event.PreviousDate, TimeSpan.Zero));

            context.MovementDateHistories.Add(movementDateHistory);

            await context.SaveChangesAsync();
        }
    }
}