namespace EA.Iws.RequestHandlers.NotificationMovements
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;

    internal class GetRejectedMovementDetailsHandler : IRequestHandler<GetRejectedMovementDetails, RejectedMovementDetails>
    {
        private readonly IMovementRejectionRepository repository;

        public GetRejectedMovementDetailsHandler(IMovementRejectionRepository repository)
        {
            this.repository = repository;
        }

        public async Task<RejectedMovementDetails> HandleAsync(GetRejectedMovementDetails message)
        {
            var details = await repository.GetByMovementId(message.MovementId);

            return new RejectedMovementDetails
            {
                Number = message.Number,
                Date = details.Date,
                Reason = details.Reason,
                FurtherDetails = details.Reason
            };
        }
    }
}
