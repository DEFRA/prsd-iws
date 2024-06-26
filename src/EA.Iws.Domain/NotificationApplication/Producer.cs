﻿namespace EA.Iws.Domain.NotificationApplication
{
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Producer : Entity
    {
        private ProducerBusiness business;
        private Address address;
        private Contact contact;

        internal Producer(ProducerBusiness business, Address address, Contact contact)
        {
            Business = business;
            Address = address;
            Contact = contact;
        }

        protected Producer()
        {
        }

        public bool IsSiteOfExport { get; internal set; }

        public ProducerBusiness Business
        {
            get { return business; }
            set
            {
                Guard.ArgumentNotNull(() => value, value);
                business = value;
            }
        }

        public Address Address
        {
            get { return address; }
            set
            {
                Guard.ArgumentNotNull(() => value, value);
                address = value;
            }
        }

        public Contact Contact
        {
            get { return contact; }
            set
            {
                Guard.ArgumentNotNull(() => value, value);
                contact = value;
            }
        }

        public void UpdateContactAndBusiness(Contact contact, ProducerBusiness business, Address addressData)
        {
            Guard.ArgumentNotNull(() => business, business);
            Guard.ArgumentNotNull(() => contact, contact);
            Guard.ArgumentNotNull(() => addressData, addressData);

            Business = business;
            Contact = contact;
            Address = addressData;
        }
    }
}