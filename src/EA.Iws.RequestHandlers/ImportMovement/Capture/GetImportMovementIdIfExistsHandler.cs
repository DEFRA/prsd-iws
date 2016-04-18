namespace EA.Iws.RequestHandlers.ImportMovement.Capture
{
    using System;
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;

    internal class GetImportMovementIdIfExistsHandler : IRequestHandler<GetImportMovementIdIfExists, Guid?>
    {
        private readonly IImportMovementRepository importMovementRepository;

        public GetImportMovementIdIfExistsHandler(IImportMovementRepository importMovementRepository)
        {
            this.importMovementRepository = importMovementRepository;
        }

        public async Task<Guid?> HandleAsync(GetImportMovementIdIfExists message)
        {
            var movement = await importMovementRepository.GetByNumberOrDefault(message.NotificationId, message.Number);

            return movement == null ? (Guid?)null : movement.Id;
        }
    }
}
