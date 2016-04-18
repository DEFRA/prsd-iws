namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;

    internal class IsProposedMovementDateValidHandler : IRequestHandler<IsProposedMovementDateValid, ProposedMovementDateResponse>
    {
        private readonly IMovementDateValidator validator;

        public IsProposedMovementDateValidHandler(IMovementDateValidator validator)
        {
            this.validator = validator;
        }

        public async Task<ProposedMovementDateResponse> HandleAsync(IsProposedMovementDateValid message)
        {
            var isOutsideConsentPeriod = false;
            var isOutOfRange = false;

            try
            {
                await validator.EnsureDateValid(message.NotificationId, message.ProposedDate);
            }
            catch (MovementDateOutOfRangeException)
            {
                isOutOfRange = true;
            }
            catch (MovementDateOutsideConsentPeriodException)
            {
                isOutsideConsentPeriod = true;
            }

            return new ProposedMovementDateResponse
            {
                IsOutsideConsentPeriod = isOutsideConsentPeriod,
                IsOutOfRange = isOutOfRange
            };
        }
    }
}