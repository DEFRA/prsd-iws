﻿namespace EA.Iws.Domain.NotificationApplication.Shipment
{
    using System;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ShipmentInfo : Entity
    {
        private decimal quantity;

        protected ShipmentInfo()
        {
        }

        public ShipmentInfo(Guid notificationId, ShipmentPeriod shipmentPeriod, int numberOfShipments, ShipmentQuantity shipmentQuantity, bool willSelfEnterShipmentData)
        {
            NotificationId = notificationId;
            UpdateQuantity(shipmentQuantity);
            UpdateShipmentPeriod(shipmentPeriod, NotificationStatus.NotSubmitted);
            UpdateNumberOfShipments(numberOfShipments);
            WillSelfEnterShipmentData = willSelfEnterShipmentData;
        }

        public Guid NotificationId { get; private set; }

        public int NumberOfShipments { get; private set; }

        public ShipmentPeriod ShipmentPeriod { get; private set; }

        public ShipmentQuantityUnits Units { get; private set; }

        public decimal Quantity
        {
            get { return quantity; }
            private set
            {
                Guard.ArgumentNotZeroOrNegative(() => value, value);
                quantity = decimal.Round(value, 4, MidpointRounding.AwayFromZero);
            }
        }
        
        public bool WillSelfEnterShipmentData { get; private set; }

        public void UpdateQuantity(ShipmentQuantity shipmentQuantity)
        {
            Quantity = shipmentQuantity.Quantity;
            Units = shipmentQuantity.Units;
        }

        public void UpdateShipmentPeriod(ShipmentPeriod shipmentPeriod, NotificationStatus status)
        {
            if (shipmentPeriod.FirstDate < SystemTime.UtcNow.Date && status == NotificationStatus.NotSubmitted)
            {
                throw new InvalidOperationException(string.Format(
                    "The start date cannot be in the past on shipment info {0}", Id));
            }

            ShipmentPeriod = shipmentPeriod;
        }

        public void UpdateNumberOfShipments(int numberOfShipments)
        {
            Guard.ArgumentNotZeroOrNegative(() => numberOfShipments, numberOfShipments);

            NumberOfShipments = numberOfShipments;
        }

        public void UpdateWillSelfEnterShipmentData(bool willSelfEnterShipmentData)
        {
            WillSelfEnterShipmentData = willSelfEnterShipmentData;
        }
    }
}