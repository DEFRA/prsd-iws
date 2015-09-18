namespace EA.Iws.RequestHandlers.Movement
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetActiveMovementsWithoutReceiptCertificateByNotificationIdHandler : IRequestHandler<GetActiveMovementsWithoutReceiptCertificateByNotificationId, IList<MovementData>>
    {
        private readonly IwsContext context;
        private readonly IMap<Movement, MovementData> mapper;
        private readonly ActiveMovementService activeMovementService;

        public GetActiveMovementsWithoutReceiptCertificateByNotificationIdHandler(IwsContext context, 
            IMap<Movement, MovementData> mapper,
            ActiveMovementService activeMovementService)
        {
            this.context = context;
            this.mapper = mapper;
            this.activeMovementService = activeMovementService;
        }

        public async Task<IList<MovementData>> HandleAsync(GetActiveMovementsWithoutReceiptCertificateByNotificationId message)
        {
            var movements = await context.GetMovementsForNotificationAsync(message.Id);

            return activeMovementService.ActiveMovements(movements)
                .Select(mapper.Map)
                .ToArray();
        }
    }
}
