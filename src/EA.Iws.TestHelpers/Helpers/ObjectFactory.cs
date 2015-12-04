namespace EA.Iws.TestHelpers.Helpers
{
    using Domain;
    using Domain.NotificationApplication;

    public static class ObjectFactory
    {
        public static Contact CreateEmptyContact()
        {
            return new Contact(string.Empty, string.Empty, string.Empty, string.Empty);
        }

        public static Business CreateEmptyBusiness()
        {
            return Business.CreateBusiness("test", BusinessType.SoleTrader, "test", "test");
        }

        public static ProducerBusiness CreateEmptyProducerBusiness()
        {
            return ProducerBusiness.CreateProducerBusiness("test", BusinessType.SoleTrader, "test", null);
        }

        public static Address CreateDefaultAddress()
        {
            return new Address("address1", string.Empty, "town", string.Empty, string.Empty, "country");
        }
    }
}