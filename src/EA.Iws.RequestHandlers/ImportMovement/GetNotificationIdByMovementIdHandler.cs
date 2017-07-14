namespace EA.Iws.RequestHandlers.ImportMovement
{
    using System;
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement;

    internal class GetNotificationIdByMovementIdHandler : IRequestHandler<GetNotificationIdByMovementId, Guid>
    {
        private readonly IImportMovementRepository importMovementRepository;

        public GetNotificationIdByMovementIdHandler(IImportMovementRepository importMovementRepository)
        {
            this.importMovementRepository = importMovementRepository;
        }

        public async Task<Guid> HandleAsync(GetNotificationIdByMovementId message)
        {
            var movement = await importMovementRepository.Get(message.MovementId);
            return movement.NotificationId;
        }
    }
}