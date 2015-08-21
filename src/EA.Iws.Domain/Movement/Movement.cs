namespace EA.Iws.Domain.Movement
{
    using System;
    using Prsd.Core.Domain;

    public class Movement : Entity
    {
        public int Number { get; private set; }

        public DateTime Date { get; private set; }

        public Guid NotificationApplicationId { get; private set; }

        protected Movement()
        {
        }

        public Movement(Guid notificationApplicationId, INotificationMovementService movementService)
        {
            if (!movementService.CanCreateNewMovementForNotification(notificationApplicationId))
            {
                throw new InvalidOperationException("Cannot create new movement for notification " + notificationApplicationId);
            }

            NotificationApplicationId = notificationApplicationId;
            Number = movementService.GetNextMovementNumber(notificationApplicationId);
        }
    }
}
