namespace EA.Iws.Domain.Movement
{
    using System;
    using NotificationApplication;
    using Prsd.Core.Domain;

    public class Movement : Entity
    {
        protected Movement()
        {
        }

        internal Movement(NotificationApplication notificationApplication, int movementNumber)
        {
            NotificationApplicationId = notificationApplication.Id;
            Number = movementNumber;
        }

        public int Number { get; private set; }

        public NotificationApplication NotificationApplication { get; private set; }

        public DateTime Date { get; private set; }

        public Guid NotificationApplicationId { get; private set; }

        public decimal Quantity { get; private set; }

        public void UpdateDate(DateTime date)
        {
            if (date >= NotificationApplication.ShipmentInfo.FirstDate
                && date <= NotificationApplication.ShipmentInfo.LastDate)
            {
                this.Date = date;
            }
            else
            {
                throw new InvalidOperationException(
                    "The date is not within the shipment date range for this notification " + date);
            }
        }
    }
}