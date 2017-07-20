namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.NotificationAssessment;
    using NotificationAssessment;
    using Prsd.Core;
    using Prsd.Core.Domain;
  
    [AutoRegister]
    public class CapturedMovementFactory : ICapturedMovementFactory
    {
        private readonly IMovementNumberValidator movementNumberValidator;
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly IUserContext userContext;

        public CapturedMovementFactory(IMovementNumberValidator movementNumberValidator, INotificationAssessmentRepository assessmentRepository,
            IUserContext userContext)
        {
            this.movementNumberValidator = movementNumberValidator;
            this.assessmentRepository = assessmentRepository;
            this.userContext = userContext;
        }

        public async Task<Movement> Create(Guid notificationId, int number, DateTime? prenotificationDate, DateTime actualShipmentDate, bool hasNoPrenotification)
        {
            if (hasNoPrenotification && prenotificationDate.HasValue)
            {
                throw new ArgumentException("Can't provide prenotification date if there is no prenotification", "prenotificationDate");
            }

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

            if (prenotificationDate.HasValue)
            {
                if (prenotificationDate > SystemTime.UtcNow.Date)
                {
                    throw new InvalidOperationException("The prenotification date cannot be in the future.");
                }
                if (actualShipmentDate < prenotificationDate)
                {
                    throw new InvalidOperationException("The actual date of shipment cannot be before the prenotification date.");
                }
                if (actualShipmentDate > prenotificationDate.Value.AddDays(60))
                {
                    throw new InvalidOperationException("The actual date of shipment should not be more than 30 calendar days after the prenotification date.");
                }
            }

            var movement = Movement.Capture(number, notificationId, actualShipmentDate, prenotificationDate, hasNoPrenotification, userContext.UserId);

            return movement;
        }
    }
}