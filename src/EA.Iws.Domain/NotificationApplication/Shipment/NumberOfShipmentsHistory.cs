﻿namespace EA.Iws.Domain.NotificationApplication.Shipment
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class NumberOfShipmentsHistory : Entity
    {
        protected NumberOfShipmentsHistory()
        {
        }

        public NumberOfShipmentsHistory(Guid notificationId, int numberOfShipments, DateTime dateChanged)
        {
            Guard.ArgumentNotDefaultValue(() => notificationId, notificationId);
            Guard.ArgumentNotDefaultValue(() => dateChanged, dateChanged);
            Guard.ArgumentNotZeroOrNegative(() => numberOfShipments, numberOfShipments);

            NotificationId = notificationId;
            NumberOfShipments = numberOfShipments;
            DateChanged = dateChanged;
        }

        public Guid NotificationId { get; private set; }

        public int NumberOfShipments { get; private set; }

        public DateTimeOffset DateChanged { get; private set; }
    }
}
