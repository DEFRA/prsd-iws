namespace EA.Iws.RequestHandlers.Movement
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetShipmentDateDataByMovementIdHandler : IRequestHandler<GetShipmentDateDataByMovementId, MovementDatesData>
    {
        private readonly IwsContext context;

        public GetShipmentDateDataByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<MovementDatesData> HandleAsync(GetShipmentDateDataByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);

            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == movement.NotificationId);

            return new MovementDatesData
            {
                MovementId = message.MovementId,
                FirstDate = notification.ShipmentInfo.FirstDate,
                LastDate = notification.ShipmentInfo.LastDate,
                ActualDate = movement.Date
            };
        }
    }
}
