namespace EA.Iws.RequestHandlers.Movement
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.MovementOperation;
    using Core.Shared;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetActiveMovementsWithReceiptCertificateByNotificationIdHandler : IRequestHandler<GetActiveMovementsWithReceiptCertificateByNotificationId, MovementOperationData>
    {
        private readonly IwsContext context;
        private readonly IMap<Movement, MovementData> mapper;
        private readonly ReceivedMovements receivedMovementsService;

        public GetActiveMovementsWithReceiptCertificateByNotificationIdHandler(IwsContext context,
            IMap<Movement, MovementData> mapper,
            ReceivedMovements receivedMovementsService)
        {
            this.context = context;
            this.mapper = mapper;
            this.receivedMovementsService = receivedMovementsService;
        }

        public async Task<MovementOperationData> HandleAsync(
            GetActiveMovementsWithReceiptCertificateByNotificationId message)
        {
            var movements = await context.GetMovementsForNotificationAsync(message.Id);
            var receivedActiveMovements = receivedMovementsService.ListActive(movements);

            var receivedMovementDatas = receivedActiveMovements.Select(mapper.Map).ToArray();

            var notification = await context.GetNotificationApplication(message.Id);

            return new MovementOperationData
            {
                MovementDatas = receivedMovementDatas,
                NotificationType = (NotificationType)notification.NotificationType.Value
            };
        }
    }
}
