namespace EA.Iws.Domain.Movement
{
    using System;
    using Core.Shared;

    public class NotificationMovementsSummary
    {
        public Guid NotificationId { get; private set; }
        public string NotificationNumber { get; private set; }
        public NotificationType NotificationType { get; private set; }
        public int IntendedTotalShipments { get; private set; }
        public int CurrentTotalShipments { get; private set; }
        public int ActiveLoadsPermitted { get; private set; }
        public int CurrentActiveLoads { get; private set; }
        public decimal IntendedQuantity { get; private set; }
        public decimal QuantityReceived { get; private set; }
        public decimal QuantityRemaining
        {
            get { return IntendedQuantity - QuantityReceived; }
        }
        public ShipmentQuantityUnits Units { get; private set; }

        public static NotificationMovementsSummary Load(Guid notificationId,
            string notificationNumber,
            NotificationType notificationType,
            int intendedTotalShipments,
            int currentTotalShipments,
            int activeLoadsPermitted,
            int currentActiveLoads,
            decimal intendedQuantity,
            decimal quantityReceived,
            ShipmentQuantityUnits units)
        {
            return new NotificationMovementsSummary
            {
                NotificationId = notificationId,
                NotificationNumber = notificationNumber,
                NotificationType = notificationType,
                IntendedTotalShipments = intendedTotalShipments,
                CurrentTotalShipments = currentTotalShipments,
                ActiveLoadsPermitted = activeLoadsPermitted,
                CurrentActiveLoads = currentActiveLoads,
                IntendedQuantity = intendedQuantity,
                QuantityReceived = quantityReceived,
                Units = units
            };
        }
    }
}
