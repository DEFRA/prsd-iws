namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.NotificationAssessment;
    using NotificationAssessment;

    [AutoRegister]
    public class CapturedMovementFactory : ICapturedMovementFactory
    {
        private readonly IMovementNumberValidator movementNumberValidator;
        private readonly INotificationAssessmentRepository assessmentRepository;

        public CapturedMovementFactory(IMovementNumberValidator movementNumberValidator, INotificationAssessmentRepository assessmentRepository)
        {
            this.movementNumberValidator = movementNumberValidator;
            this.assessmentRepository = assessmentRepository;
        }

        public async Task<Movement> Create(Guid notificationId, int number, DateTime? prenotificationDate, DateTime actualShipmentDate)
        {
            if (!await movementNumberValidator.Validate(notificationId, number))
            {
                throw new MovementNumberException("Cannot create a movement with a conflicting movement number (" + number + ") for notification: " + notificationId);
            }

            var notificationStatus = (await assessmentRepository.GetByNotificationId(notificationId)).Status;

            if (notificationStatus != NotificationStatus.Consented)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create a movement for notification {0} because its status is {1}",
                        notificationId, notificationStatus));
            }

            var movement = Movement.Capture(number, notificationId, actualShipmentDate, prenotificationDate);

            return movement;
        }
    }
}