namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.NotificationAssessment;
    using NotificationApplication;
    using NotificationAssessment;

    public class MovementFactory
    {
        public Movement Create(NotificationApplication notificationApplication, 
            NotificationAssessment notificationAssessment, 
            ICollection<Movement> notificationMovements)
        {
            if (notificationAssessment.Status == NotificationStatus.NotSubmitted)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create a new movement for notification {0} which has status {1}", 
                    notificationApplication.Id, 
                    notificationAssessment.Status));
            }

            int currentNumberOfMovements = notificationMovements.Count;

            if (currentNumberOfMovements == notificationApplication.ShipmentInfo.NumberOfShipments)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot add a movement to notification {0} which has {1} of {2} movements",
                    notificationApplication.Id,
                    currentNumberOfMovements,
                    notificationApplication.ShipmentInfo.NumberOfShipments));
            }

            return new Movement(notificationApplication, currentNumberOfMovements + 1);
        }
    }
}
