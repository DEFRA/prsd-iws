namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Linq;
    using Core.NotificationAssessment;
    using Core.Shared;
    using DataAccess;
    using Domain;

    internal class NotificationMovementService : INotificationMovementService
    {
        private readonly IwsContext context;

        public NotificationMovementService(IwsContext context)
        {
            this.context = context;
        }

        public int GetNextMovementNumber(Guid notificationId)
        {
            if (!context.NotificationApplications.Any(na => na.Id == notificationId))
            {
                throw new InvalidOperationException("Cannot get next movement number for non-existent notification " + notificationId);
            }

            return context.Movements.Count(m => m.NotificationApplicationId == notificationId) + 1;
        }

        public bool DateIsValid(Guid notificationId, DateTime date)
        {
            var notification = context.NotificationApplications.Single(na => na.Id == notificationId);

            bool isGreaterThanOrEqualToFirstDate = date >= notification.ShipmentInfo.FirstDate;
            bool isLessThanOrEqualToLastDate = date <= notification.ShipmentInfo.LastDate;

            return isGreaterThanOrEqualToFirstDate && isLessThanOrEqualToLastDate;
        }
    }
}
