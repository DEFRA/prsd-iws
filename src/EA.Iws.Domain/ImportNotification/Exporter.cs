﻿namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Exporter : Entity
    {
        protected Exporter()
        {
        }

        public Exporter(Guid importNotificationId, string businessName, Address address, Contact contact)
        {
            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);
            Guard.ArgumentNotNullOrEmpty(() => businessName, businessName);
            Guard.ArgumentNotNull(() => address, address);
            Guard.ArgumentNotNull(() => contact, contact);

            ImportNotificationId = importNotificationId;
            Name = businessName;
            Address = address;
            Contact = contact;
        }

        public Guid ImportNotificationId { get; private set; }

        public string Name { get; private set; }

        public Address Address { get; private set; }

        public Contact Contact { get; private set; }

        public void UpdateContactAndName(Contact contact, string businessName)
        {
            Guard.ArgumentNotNull(() => contact, contact);
            Guard.ArgumentNotNullOrEmpty(() => businessName, businessName);
            Contact = contact;
            Name = businessName;
        }
    }
}