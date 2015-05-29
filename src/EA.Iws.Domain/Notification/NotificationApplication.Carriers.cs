namespace EA.Iws.Domain.Notification
{
    public partial class NotificationApplication
    {
        public Carrier AddCarrier(Business business, Address address, Contact contact)
        {
            var carrier = new Carrier(business, address, contact);
            CarriersCollection.Add(carrier);
            return carrier;
        }
    }
}