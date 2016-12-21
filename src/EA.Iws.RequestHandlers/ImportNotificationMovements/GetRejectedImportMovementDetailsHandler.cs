namespace EA.Iws.RequestHandlers.ImportNotificationMovements
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationMovements;

    internal class GetRejectedImportMovementDetailsHandler : IRequestHandler<GetRejectedImportMovementDetails, RejectedMovementDetails>
    {
        private readonly IImportMovementRejectionRepository repository;

        public GetRejectedImportMovementDetailsHandler(IImportMovementRejectionRepository repository)
        {
            this.repository = repository;
        }

        public async Task<RejectedMovementDetails> HandleAsync(GetRejectedImportMovementDetails message)
        {
            var details = await repository.GetByMovementIdOrDefault(message.MovementId);

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
