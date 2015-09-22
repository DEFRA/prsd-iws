namespace EA.Iws.RequestHandlers.Movement
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.MeansOfTransport;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetMeansOfTransportByMovementIdHandler : IRequestHandler<GetMeansOfTransportByMovementId, IList<MeansOfTransport>>
    {
        private readonly IwsContext context;

        public GetMeansOfTransportByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IList<MeansOfTransport>> HandleAsync(GetMeansOfTransportByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.Id);
            var notification = await context.GetNotificationApplication(movement.NotificationId);

            return notification.MeansOfTransport.ToArray();
        }
    }
}
