namespace EA.Iws.RequestHandlers.ImportNotificationMovements
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationMovements;

    internal class GetRejectedImportMovementDetailsHandler : IRequestHandler<GetRejectedImportMovementDetails, RejectedMovementDetails>
    {
        private readonly IImportMovementRejectionRepository rejectionRepository;
        private readonly IImportMovementRepository movementRepository;

        public GetRejectedImportMovementDetailsHandler(IImportMovementRejectionRepository rejectionRepository, IImportMovementRepository movementRepository)
        {
            this.rejectionRepository = rejectionRepository;
            this.movementRepository = movementRepository;
        }

        public async Task<RejectedMovementDetails> HandleAsync(GetRejectedImportMovementDetails message)
        {
            var movement = await movementRepository.Get(message.MovementId);
            var details = await rejectionRepository.GetByMovementIdOrDefault(message.MovementId);

            return new RejectedMovementDetails
            {
                Number = movement.Number,
                Date = details.Date,
                Reason = details.Reason,
                FurtherDetails = details.FurtherDetails
            };
        }
    }
}
