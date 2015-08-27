namespace EA.Iws.Domain.Movement
{
    using System.Collections.Generic;
    using NotificationApplication;
    using NotificationAssessment;

    public interface IMovementFactory
    {
        Movement Create(NotificationApplication notificationApplication, 
            NotificationAssessment notificationAssessment, 
            ICollection<Movement> notificationMovements);
    }
}
