namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetNotificationIdByMovementIdHandler : IRequestHandler<GetNotificationIdByMovementId, Guid>
    {
        private readonly IwsContext context;

        public GetNotificationIdByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(GetNotificationIdByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);

            return movement.NotificationApplicationId;
        }
    }
}
