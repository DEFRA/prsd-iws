namespace EA.Iws.RequestHandlers.Movement.Edit
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Domain.NotificationConsent;
    using Prsd.Core.Mediator;
    using Requests.Movement.Edit;

    internal class GetMaximumValidMovementDateHandler : IRequestHandler<GetMaximumValidMovementDate, ValidMovementDates>
    {
        private readonly IMovementRepository movementRepository;
        private readonly INotificationConsentRepository consentRepository;
        private readonly ValidMovementDateCalculator validMovementDate;

        public GetMaximumValidMovementDateHandler(ValidMovementDateCalculator validMovementDate,
            IMovementRepository movementRepository,
            INotificationConsentRepository consentRepository)
        {
            this.validMovementDate = validMovementDate;
            this.consentRepository = consentRepository;
            this.movementRepository = movementRepository;
        }

        public async Task<ValidMovementDates> HandleAsync(GetMaximumValidMovementDate message)
        {
            var maxDate = await validMovementDate.Maximum(message.MovementId);
            var movement = await movementRepository.GetById(message.MovementId);
            var consent = await consentRepository.GetByNotificationId(movement.NotificationId);

            return new ValidMovementDates
            {
                MaxValidDate = maxDate,
                ConsentStart = consent.ConsentRange.From,
                ConsentEnd = consent.ConsentRange.To
            };
        }
    }
}