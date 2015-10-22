namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.NotificationAssessment;
    using NotificationApplication;
    using NotificationApplication.Shipment;
    using NotificationAssessment;

    public class MovementFactory
    {
        public Movement Create(NotificationApplication notificationApplication, 
            NotificationAssessment notificationAssessment,
            ShipmentInfo shipmentInfo,
            ICollection<Movement> notificationMovements)
        {
            if (notificationAssessment.Status != NotificationStatus.Consented)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create a new movement for notification {0} which has status {1}", 
                    notificationApplication.Id, 
                    notificationAssessment.Status));
            }

            int currentNumberOfMovements = notificationMovements.Count;

            if (currentNumberOfMovements == shipmentInfo.NumberOfShipments)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot add a movement to notification {0} which has {1} of {2} movements",
                    notificationApplication.Id,
                    currentNumberOfMovements,
                    shipmentInfo.NumberOfShipments));
            }

            return new Movement(currentNumberOfMovements + 1, notificationApplication.Id);
        }
    }
}
