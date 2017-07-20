namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.ImportNotificationAssessment;
    using ImportNotification;
    using Movement;
    using Prsd.Core;

    [AutoRegister]
    public class ImportMovementFactory : IImportMovementFactory
    {
        private readonly IImportMovementNumberValidator numberValidator;
        private readonly IImportNotificationAssessmentRepository assessmentRepository;

        public ImportMovementFactory(IImportMovementNumberValidator numberValidator, 
            IImportNotificationAssessmentRepository assessmentRepository)
        {
            this.numberValidator = numberValidator;
            this.assessmentRepository = assessmentRepository;
        }

        public async Task<ImportMovement> Create(Guid notificationId, int number, DateTime actualShipmentDate, DateTime? prenotificationDate)
        {
            if (!await numberValidator.Validate(notificationId, number))
            {
                throw new MovementNumberException("Cannot create an import movement with a conflicting movement number (" + number + ") for import notification: " + notificationId);
            }

            var notificationStatus = (await assessmentRepository.GetByNotification(notificationId)).Status;

            if (notificationStatus != ImportNotificationStatus.Consented)
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

            return new ImportMovement(notificationId, number, actualShipmentDate);
        }
    }
}