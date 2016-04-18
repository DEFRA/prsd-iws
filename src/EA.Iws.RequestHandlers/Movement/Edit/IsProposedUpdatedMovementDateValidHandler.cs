namespace EA.Iws.RequestHandlers.Movement.Edit
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement.Edit;

    internal class IsProposedUpdatedMovementDateValidHandler : IRequestHandler<IsProposedUpdatedMovementDateValid, ProposedUpdatedMovementDateResponse>
    {
        private readonly IMovementRepository repository;
        private readonly IUpdatedMovementDateValidator validator;

        public IsProposedUpdatedMovementDateValidHandler(IUpdatedMovementDateValidator validator,
            IMovementRepository repository)
        {
            this.validator = validator;
            this.repository = repository;
        }

        public async Task<ProposedUpdatedMovementDateResponse> HandleAsync(IsProposedUpdatedMovementDateValid message)
        {
            var isOutsideConsentPeriod = false;
            var isOutOfRange = false;
            var isOutOfRangeOfOriginalDate = false;

            var movement = await repository.GetById(message.MovementId);

            try
            {
                await validator.EnsureDateValid(movement, message.ProposedDate);
            }
            catch (MovementDateOutsideConsentPeriodException)
            {
                isOutsideConsentPeriod = true;
            }
            catch (MovementDateOutOfRangeException)
            {
                isOutOfRange = true;
            }
            catch (MovementDateOutOfRangeOfOriginalDateException)
            {
                isOutOfRangeOfOriginalDate = true;
            }

            return new ProposedUpdatedMovementDateResponse
            {
                IsOutsideConsentPeriod = isOutsideConsentPeriod,
                IsOutOfRange = isOutOfRange,
                IsOutOfRangeOfOriginalDate = isOutOfRangeOfOriginalDate
            };
        }
    }
}