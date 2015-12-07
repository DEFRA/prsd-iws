namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class EmailAddressTests
    {
        public static IEnumerable<object[]> ValidEmails()
        {
            return new[]
            {
                new object[] { "email@example.com" },
                new object[] { "firstname.lastname@example.com" },
                new object[] { "email@subdomain.example.com" },
                new object[] { "firstname+lastname@example.com" },
                new object[] { "“email”@example.com" },
                new object[] { "1234567890@example.com" },
                new object[] { "email@example-one.com" },
                new object[] { "_______@example.com" },
                new object[] { "email@example.name" },
                new object[] { "email@example.museum" },
                new object[] { "email@example.co.jp" },
                new object[] { "firstname-lastname@example.com" }
            };
        }

        public static IEnumerable<object[]> InvalidEmails()
        {
            return new[]
            {
                new object[] { "plainaddress" },
                new object[] { "#@%^%#$@#$@#.com" },
                new object[] { "@example.com" },
                new object[] { "Joe Smith < email@example.com >" },
                new object[] { "email.example.com" },
                new object[] { "email @example@example.com" },
                new object[] { ".email @example.com" },
                new object[] { "email.@example.com" },
                new object[] { "email..email@example.com" },
                new object[] { "email@example.com(Joe Smith)" },
                new object[] { "email @example" },
                new object[] { "email@-example.com" },
                new object[] { "email @example.web" },
                new object[] { "email@111.222.333.44444" },
                new object[] { "email @example..com" },
                new object[] { "Abc..123@example.com" },
                new object[] { @"“(),:;<>[\]@example.com" },
                new object[] { @"just""not""right@example.com" },
                new object[] { @"this\ is""really""not\allowed@example.com" }
            };
        }
            
        [Theory]
        [MemberData("ValidEmails")]
        public void ValidEmailAddresses(string input)
        {
            var email = new EmailAddress(input);

            Assert.IsType<EmailAddress>(email);
        }
            
        [Theory]
        [MemberData("InvalidEmails")]
        public void InvalidEmailAddresses(string input)
        {
            Action createEmail = () => new EmailAddress(input);

            Assert.Throws<ArgumentException>(createEmail);
        }

        [Fact]
        public void EqualsOperator()
        {
            var email1 = new EmailAddress("email@address.com");
            var email2 = new EmailAddress("email@address.com");

            Assert.True(email1 == email2);
        }

        [Fact]
        public void EqualsMethod()
        {
            var email1 = new EmailAddress("email@address.com");
            var email2 = new EmailAddress("email@address.com");

            Assert.True(email1.Equals(email2));
        }

        [Fact]
        public void ToStringReturnsValue()
        {
            var email = new EmailAddress("email@address.com");

            Assert.Equal("email@address.com", email.ToString());
        }

        [Fact]
        public void NotEqualsOperator()
        {
            var email1 = new EmailAddress("email@address.com");
            var email2 = new EmailAddress("email2@address.com");

            Assert.True(email1 != email2);
        }

        [Fact]
        public void StringOperator()
        {
            var email = new EmailAddress("email@address.com");

            Assert.True("email@address.com" == email);
        }
    }
}