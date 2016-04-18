namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EA.Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class CarrierCollection : Entity
    {
        protected CarrierCollection()
        {
        }

        public CarrierCollection(Guid notificationId)
        {
            NotificationId = notificationId;

            CarriersCollection = new List<Carrier>();
        }

        public Guid NotificationId { get; private set; }

        protected virtual ICollection<Carrier> CarriersCollection { get; set; }

        public IEnumerable<Carrier> Carriers
        {
            get { return CarriersCollection.ToSafeIEnumerable(); }
        }

        public Carrier AddCarrier(Business business, Address address, Contact contact)
        {
            var carrier = new Carrier(business, address, contact);
            CarriersCollection.Add(carrier);
            return carrier;
        }

        public Carrier GetCarrier(Guid carrierId)
        {
            var carrier = CarriersCollection.SingleOrDefault(p => p.Id == carrierId);
            if (carrier == null)
            {
                throw new InvalidOperationException(
                    string.Format("Carrier with id {0} does not exist on this notification {1}", carrierId, Id));
            }
            return carrier;
        }

        public void RemoveCarrier(Guid carrierId)
        {
            var carrier = GetCarrier(carrierId);

            CarriersCollection.Remove(carrier);
        }
    }
}