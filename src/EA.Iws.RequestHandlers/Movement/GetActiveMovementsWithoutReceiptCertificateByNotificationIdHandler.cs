namespace EA.Iws.RequestHandlers.Movement
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Prsd.Core;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Movement = Domain.Movement.Movement;

    internal class GetActiveMovementsWithoutReceiptCertificateByNotificationIdHandler : IRequestHandler<GetActiveMovementsWithoutReceiptCertificateByNotificationId, IList<MovementData>>
    {
        private readonly IwsContext context;
        private readonly IMap<Movement, MovementData> mapper;

        public GetActiveMovementsWithoutReceiptCertificateByNotificationIdHandler(IwsContext context, IMap<Movement, MovementData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IList<MovementData>> HandleAsync(GetActiveMovementsWithoutReceiptCertificateByNotificationId message)
        {
            var movements = await context.Movements.Where(m => m.NotificationApplicationId == message.Id
                && m.Date.HasValue
                && m.Date <= SystemTime.UtcNow).ToArrayAsync();

            return movements.Select(mapper.Map).ToArray();
        }
    }
}
