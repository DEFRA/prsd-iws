namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using Domain;

    internal class TestService : INotificationMovementService 
    {
        public bool CanCreate { get; set; }

        public int NextNumber { get; set; }

        public bool CanCreateNewMovementForNotification(Guid notificationId)
        {
            return CanCreate;
        }

        public int GetNextMovementNumber(Guid notificationApplicationId)
        {
            return NextNumber;
        }
    }
}