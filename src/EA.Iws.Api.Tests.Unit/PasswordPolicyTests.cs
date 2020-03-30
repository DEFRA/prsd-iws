namespace EA.Iws.Api.Tests.Unit
{
    using FluentAssertions;
    using Identity;
    using Xunit;

    public class PasswordPolicyTests
    {
        private readonly PasswordPolicy passwordPolicy;
        private const string ErrorString = "Please check that your password has at least 8 characters and contains at least one upper case letter, one lower case letter, 1 special character and one number. The password cannot be the same as the email address.";

        public PasswordPolicyTests()
        {
            passwordPolicy = new PasswordPolicy();
        }

        [Fact]
        public async void ValidateAsync_GivenNullItem_ArgumentNullExceptionExpected()
        {
            var result = await Xunit.Record.ExceptionAsync(() => passwordPolicy.ValidateAsync(null));

            result.Should().BeOfType<System.ArgumentNullException>();
        }

        [Fact]
        public async void ValidateAsync_GivenPasswordLengthIsLessThanRequired_ResultShouldContainError()
        {
            var result = await passwordPolicy.ValidateAsync("PaswoS1");

            result.Errors.Should().Contain(ErrorString);
        }

        [Fact]
        public async void ValidateAsync_GivenPasswordDoesNotContainLower_ResultShouldContainError()
        {
            var result = await passwordPolicy.ValidateAsync("PA5SW&RD");

            result.Errors.Should().Contain(ErrorString);
        }

        [Fact]
        public async void ValidateAsync_GivenPasswordDoesNotContainUpper_ResultShouldContainError()
        {
            var result = await passwordPolicy.ValidateAsync("pa5sw&rd");

            result.Errors.Should().Contain(ErrorString);
        }

        [Fact]
        public async void ValidateAsync_GivenPasswordDoesNotContainDigit_ResultShouldContainError()
        {
            var result = await passwordPolicy.ValidateAsync("pa5sw&rd");

            result.Errors.Should().Contain(ErrorString);
        }

        [Fact]
        public async void ValidateAsync_GivenPasswordDoesNotContainSpecial_ResultShouldContainError()
        {
            var result = await passwordPolicy.ValidateAsync("pa5sword");

            result.Errors.Should().Contain(ErrorString);
        }

        [Fact]
        public async void ValidateAsync_GivenPasswordContainsUpperLowerDigitAndSpecial_ResultShouldBeSucceeded()
        {
            var result = await passwordPolicy.ValidateAsync("Passw0r&");

            result.Succeeded.Should().BeTrue();
        }
    }
}
