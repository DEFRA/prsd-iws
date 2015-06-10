namespace EA.Iws.Domain.Tests.Unit.Helpers
{
    using System;

    public static class ObjectFactory
    {
        public static Contact CreateEmptyContact()
        {
            return new Contact(string.Empty, String.Empty, String.Empty, String.Empty);
        }

        public static Business CreateEmptyBusiness()
        {
            return new Business(string.Empty, String.Empty, String.Empty, string.Empty);
        }

        public static Address CreateEmptyAddress()
        {
            return new Address(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty);
        }
    }
}