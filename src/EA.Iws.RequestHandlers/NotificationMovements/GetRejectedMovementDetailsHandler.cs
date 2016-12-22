namespace EA.Iws.RequestHandlers.NotificationMovements
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;

    internal class GetRejectedMovementDetailsHandler : IRequestHandler<GetRejectedMovementDetails, RejectedMovementDetails>
    {
        private readonly IMovementRejectionRepository rejectionRepository;
        private readonly IMovementRepository movementRepository;

        public GetRejectedMovementDetailsHandler(IMovementRejectionRepository rejectionRepository, IMovementRepository movementRepository)
        {
            this.rejectionRepository = rejectionRepository;
            this.movementRepository = movementRepository;
        }

        public async Task<RejectedMovementDetails> HandleAsync(GetRejectedMovementDetails message)
        {
            var movement = await movementRepository.GetById(message.MovementId);
            var details = await rejectionRepository.GetByMovementId(message.MovementId);

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
