namespace EA.Iws.Web.Tests.Unit.ViewModels.Registration
{
    using FluentAssertions;
    using Web.ViewModels.Registration;
    using Xunit;

    public class ApplicantRegistrationViewModelTests
    {
        [Fact]
        public void UserCreationViewModel_ShouldInheritFromCreateUserModelBase()
        {
            typeof(ApplicantRegistrationViewModel).Should().BeDerivedFrom<CreateUserModelBase>();
        }
    }
}
