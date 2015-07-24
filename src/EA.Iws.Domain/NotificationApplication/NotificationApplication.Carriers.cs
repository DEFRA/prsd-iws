namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Linq;

    public partial class NotificationApplication
    {
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