namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class PhoneNumberTests
    {
        public static IEnumerable<object[]> ValidPhoneNumbers()
        {
            return new[]
            {
                new object[] { "+44 1234 567 890" },
                new object[] { "01234 567890" },
                new object[] { "0202 123 1234" },
                new object[] { "0044-1234-567890" },
                new object[] { "01234567890" }
            };
        }

        public static IEnumerable<object[]> InvalidPhoneNumbers()
        {
            return new[]
            {
                new object[] { "++44 1232 123412" },
                new object[] { "01234 567890 ext. 123" },
                new object[] { "something" },
                new object[] { "01234+1232+1233" },
                new object[] { "0202  123 1234" },
                new object[] { "02345  567890" },
                new object[] { "-1234 456456" },
                new object[] { "01234 567890-" },
                new object[] { "01234--123123" },
                new object[] { "01234 - 123123" },
                new object[] { "01234 -1232123" },
                new object[] { "012324- 1232123" },
                new object[] { "01234556782+" }
            };
        }

        [Theory]
        [MemberData("ValidPhoneNumbers")]
        public void ValidTelephoneNumbers(string input)
        {
            var phoneNumber = new PhoneNumber(input);

            Assert.IsType<PhoneNumber>(phoneNumber);
        }

        [Theory]
        [MemberData("InvalidPhoneNumbers")]
        public void InvalidTelephoneNumbers(string input)
        {
            Action createPhoneNumber = () => new PhoneNumber(input);

            Assert.Throws<ArgumentException>(createPhoneNumber);
        }

        [Fact]
        public void EqualsOperator()
        {
            var phoneNumber1 = new PhoneNumber("01234 567890");
            var phoneNumber2 = new PhoneNumber("01234 567890");

            Assert.True(phoneNumber1 == phoneNumber2);
        }

        [Fact]
        public void EqualsMethod()
        {
            var phoneNumber1 = new PhoneNumber("01234 567890");
            var phoneNumber2 = new PhoneNumber("01234 567890");

            Assert.True(phoneNumber1.Equals(phoneNumber2));
        }

        [Fact]
        public void ToStringReturnsValue()
        {
            var phoneNumber = new PhoneNumber("01234 567890");

            Assert.Equal("01234 567890", phoneNumber.ToString());
        }

        [Fact]
        public void NotEqualsOperator()
        {
            var phoneNumber1 = new PhoneNumber("01234 567890");
            var phoneNumber2 = new PhoneNumber("01234 567899");

            Assert.True(phoneNumber1 != phoneNumber2);
        }

        [Fact]
        public void StringOperator()
        {
            var phoneNumber = new PhoneNumber("01234 567890");

            Assert.True(phoneNumber == "01234 567890");
        }
    }
}