namespace EA.Iws.RequestHandlers.Tests.Unit.Mappings
{
    using RequestHandlers.Mappings;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ContactMapTests
    {
        private const string AnyString = "test";
        private const string TestString = "Pickled peppers";
        private const string TelephoneString = "45-2121880";
        private const string Prefix = "45";
        private const string Telephone = "2121880";

        private readonly ContactMap contactMap;
        private readonly TestableContact contact = new TestableContact
        {
            Email = AnyString,
            Fax = AnyString,
            FullName = AnyString,
            Telephone = AnyString
        };

        public ContactMapTests()
        {
            contactMap = new ContactMap();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(TestString)]
        public void MapFullName(string fullName)
        {
            contact.FullName = fullName;

            var result = contactMap.Map(contact);

            Assert.Equal(fullName, result.FullName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(TestString)]
        public void MapEmail(string email)
        {
            contact.Email = email;

            var result = contactMap.Map(contact);

            Assert.Equal(email, result.Email);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", null, "")]
        [InlineData(TestString, null, TestString)]
        [InlineData(TelephoneString, Prefix, Telephone)]
        public void MapTelephone(string input, string prefix, string body)
        {
            contact.Telephone = input;

            var result = contactMap.Map(contact);

            Assert.Equal(prefix, result.TelephonePrefix);
            Assert.Equal(body, result.Telephone);
        }
    }
}