namespace EA.Iws.Web.Tests.Unit.ViewModels.Registration
{
    using Web.ViewModels.Registration;

    public class CreateUserModelTestBase : CreateUserModelBase
    {
        public override string Password { get; set; }
        public override string Email { get; set; }
    }
}
