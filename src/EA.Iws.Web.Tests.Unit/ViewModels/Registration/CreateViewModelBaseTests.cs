namespace EA.Iws.Web.Tests.Unit.ViewModels.Registration
{
    using System.ComponentModel.DataAnnotations;
    using FluentAssertions;
    using Xunit;

    public class CreateUserModelBaseTests
    {
        private readonly CreateUserModelTestBase model;

        public CreateUserModelBaseTests()
        {
            model = new CreateUserModelTestBase();
        }

        [Fact]
        public void Validate_GivenEmailAndPasswordDiffer_ValidationResultsShouldBeEmpty()
        {
            model.Email = "email";
            model.Password = "pass";

            var result = model.Validate(new ValidationContext(model));

            result.Should().BeEmpty();
        }

        [Fact]
        public void Validate_GivenEmailAndPasswordAreTheSame_ValidationResultsShouldContainValidationError()
        {
            model.Email = "email";
            model.Password = "email";

            var result = model.Validate(new ValidationContext(model));

            result.Should().SatisfyRespectively(
                item =>
                {
                    item.ErrorMessage.Should().Be("The password cannot be the same as the email address");
                    item.MemberNames.Should().Contain("Password");
                });
        }
    }
}
