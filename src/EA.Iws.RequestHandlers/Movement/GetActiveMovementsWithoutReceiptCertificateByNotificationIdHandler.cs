namespace EA.Iws.RequestHandlers.Movement
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetActiveMovementsWithoutReceiptCertificateByNotificationIdHandler : IRequestHandler<GetActiveMovementsWithoutReceiptCertificateByNotificationId, IList<MovementData>>
    {
        private readonly IwsContext context;
        private readonly IMap<Movement, MovementData> mapper;
        private readonly ActiveMovements activeMovementService;

        public GetActiveMovementsWithoutReceiptCertificateByNotificationIdHandler(IwsContext context, 
            IMap<Movement, MovementData> mapper,
            ActiveMovements activeMovementService)
        {
            this.context = context;
            this.mapper = mapper;
            this.activeMovementService = activeMovementService;
        }

        public async Task<IList<MovementData>> HandleAsync(GetActiveMovementsWithoutReceiptCertificateByNotificationId message)
        {
            var movements = await context.GetMovementsForNotificationAsync(message.Id);

            return activeMovementService.List(movements)
                .Select(mapper.Map)
                .ToArray();
        }
    }
}
