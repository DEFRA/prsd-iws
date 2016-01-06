namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using NotificationApplication;

    public class UpdatedMovementDateValidator : IUpdatedMovementDateValidator
    {
        private readonly IMovementDateValidator movementDateValidator;
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly IWorkingDayCalculator workingDayCalculator;
        private readonly OriginalMovementDate originalMovementDate;

        public UpdatedMovementDateValidator(IMovementDateValidator movementDateValidator,
            OriginalMovementDate originalMovementDate,
            IWorkingDayCalculator workingDayCalculator,
            INotificationApplicationRepository notificationRepository)
        {
            this.originalMovementDate = originalMovementDate;
            this.workingDayCalculator = workingDayCalculator;
            this.notificationRepository = notificationRepository;
            this.movementDateValidator = movementDateValidator;
        }

        public async Task EnsureDateValid(Movement movement, DateTime newDate)
        {
            if (movement.Status != MovementStatus.Submitted)
            {
                throw new MovementDateException(string.Format(
                    "Can't set new movement date {0} because the movement is not in the submitted state (state: {1})",
                    newDate,
                    movement.Status));
            }

            await movementDateValidator.EnsureDateValid(movement.NotificationId, newDate);

            var originalDate = await originalMovementDate.Get(movement);
            var notification = await notificationRepository.GetByMovementId(movement.Id);
            var includeStartDate = false;
            if (newDate > workingDayCalculator.AddWorkingDays(originalDate, 10, includeStartDate, notification.CompetentAuthority))
            {
                throw new MovementDateException(string.Format(
                    "Can't set new movement date {0} since it is more than 10 working days after the original shipment date {1}",
                    newDate,
                    originalDate));
            }
        }
    }
}