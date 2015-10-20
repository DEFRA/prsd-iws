namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetMovementDateByMovementIdHandler : IRequestHandler<GetMovementDateByMovementId, DateTime>
    {
        private readonly IwsContext context;

        public GetMovementDateByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<DateTime> HandleAsync(GetMovementDateByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);

            return movement.Date.GetValueOrDefault();
        }
    }
}
