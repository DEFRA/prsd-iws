namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.NotificationAssessment;
    using NotificationAssessment;

    [AutoRegister]
    public class MovementFactory
    {
        private readonly IMovementDateValidator dateValidator;
        private readonly MovementNumberGenerator numberGenerator;
        private readonly NumberOfMovements numberOfMovements;
        private readonly NotificationMovementsQuantity movementsQuantity;
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly NumberOfActiveLoads numberOfActiveLoads;
        private readonly ConsentPeriod consentPeriod;

        public MovementFactory(NumberOfMovements numberOfMovements,
            NotificationMovementsQuantity movementsQuantity,
            INotificationAssessmentRepository assessmentRepository,
            MovementNumberGenerator numberGenerator,
            NumberOfActiveLoads numberOfActiveLoads,
            ConsentPeriod consentPeriod,
            IMovementDateValidator dateValidator)
        {
            this.numberOfMovements = numberOfMovements;
            this.movementsQuantity = movementsQuantity;
            this.assessmentRepository = assessmentRepository;
            this.numberGenerator = numberGenerator;
            this.numberOfActiveLoads = numberOfActiveLoads;
            this.consentPeriod = consentPeriod;
            this.dateValidator = dateValidator;
        }

        public async Task<Movement> Create(Guid notificationId, DateTime actualMovementDate)
        {
            await dateValidator.EnsureDateValid(notificationId, actualMovementDate);

            var hasMaximumMovements = await numberOfMovements.HasMaximum(notificationId);

            if (hasMaximumMovements)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create new movement for notification {0} which has used all available movements",
                        notificationId));
            }
            
            var hasMaximumActiveLoads = await numberOfActiveLoads.HasMaximum(notificationId);

            if (hasMaximumActiveLoads)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create new movement for notification {0} because permitted active loads would be exceeded",
                        notificationId));
            }

            var quantityRemaining = await movementsQuantity.Remaining(notificationId);

            if (quantityRemaining <= new ShipmentQuantity(0, quantityRemaining.Units))
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create new movement for notification {0} as the quantity has been reached or exceeded.", 
                        notificationId));
            }

            var notificationStatus = (await assessmentRepository.GetByNotificationId(notificationId)).Status;

            if (notificationStatus != NotificationStatus.Consented)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create a movement for notification {0} because its status is {1}",
                        notificationId, notificationStatus));
            }

            var consentPeriodExpired = await consentPeriod.HasExpired(notificationId);

            if (consentPeriodExpired)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create new movement for notification {0} because the consent period has passed",
                        notificationId));
            }

            var newNumber = await numberGenerator.Generate(notificationId);

            return new Movement(newNumber, notificationId, actualMovementDate);
        }
    }
}