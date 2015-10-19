namespace EA.Iws.DocumentGeneration.Tests.Unit
{
    using Xunit;

    public class DocumentHelperTests
    {
        [Fact]
        public void ContactNumberIsNull_ReturnsEmpty()
        {
            string contactNumber = null;
            Assert.Empty(contactNumber.ToFormattedContact());
        }

        [Fact]
        public void ContactNumberIsBlank_ReturnsEmpty()
        {
            string contactNumber = string.Empty;
            Assert.Empty(contactNumber.ToFormattedContact());
        }

        [Fact]
        public void ContactNumberIsWhitespacesOnly_ReturnsEmpty()
        {
            string contactNumber = "   ";
            Assert.Empty(contactNumber.ToFormattedContact());
        }

        [Fact]
        public void ContactNumberIsValidContactNumber_ReturnsNonEmpty()
        {
            string contactNumber = "123456789";
            Assert.NotEmpty(contactNumber.ToFormattedContact());
        }

        [Theory]
        [InlineData("44-1234560789", "+44 1234560789")]
        [InlineData("44-123 456 0789", "+44 123 456 0789")]
        public void ContactNumberIsValidContactNumber_ReturnsFormattedContactNumber(string contactNumber, string formattedContactNumber)
        {
            var result = contactNumber.ToFormattedContact();
            Assert.NotEmpty(result);
            Assert.Equal(result, formattedContactNumber);
        }
    }
}
