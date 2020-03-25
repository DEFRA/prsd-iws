namespace EA.Iws.Web.Tests.Unit.ViewModels.Registration
{
    using Areas.Admin.ViewModels.Registration;
    using FluentAssertions;
    using Web.ViewModels.Registration;
    using Xunit;

    public class AdminRegistrationViewModelTests
    {
        [Fact]
        public void UserCreationViewModel_ShouldInheritFromCreateUserModelBase()
        {
            typeof(AdminRegistrationViewModel).Should().BeDerivedFrom<CreateUserModelBase>();
        }
    }
}
