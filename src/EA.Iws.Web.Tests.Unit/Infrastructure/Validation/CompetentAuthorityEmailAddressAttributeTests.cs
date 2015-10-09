namespace EA.Iws.Web.Tests.Unit.Infrastructure.Validation
{
    using System.Web.Mvc;
    using Core.Notification;
    using FakeItEasy;
    using Services;
    using TestHelpers;
    using Web.Infrastructure.Validation;
    using Xunit;

    public class CompetentAuthorityEmailAddressAttributeTests
    {
        private readonly TestViewModel viewModel;
        private readonly IDependencyResolver dependencyResolver;

        public CompetentAuthorityEmailAddressAttributeTests()
        {
            dependencyResolver = A.Fake<IDependencyResolver>();
            A.CallTo(() => dependencyResolver.GetService(typeof(AppConfiguration))).Returns(
                new AppConfiguration()
                {
                    Environment = "LIVE"
                });
            DependencyResolver.SetResolver(dependencyResolver);
            viewModel = new TestViewModel();
        }

        [Theory]
        [InlineData(CompetentAuthority.England, "valid@environment-agency.gov.uk")]
        [InlineData(CompetentAuthority.NorthernIreland, "valid@doeni.gov.uk")]
        [InlineData(CompetentAuthority.Scotland, "valid@sepa.org.uk")]
        [InlineData(CompetentAuthority.Wales, "valid@cyfoethnaturiolcymru.gov.uk")]
        [InlineData(CompetentAuthority.Wales, "valid@naturalresourceswales.gov.uk")]
        public void ValidAgencyEmailAddresses(CompetentAuthority competentAuthority, string email)
        {
            viewModel.CompetentAuthority = competentAuthority;
            viewModel.Email = email;
            var validationResult = ViewModelValidator.ValidateViewModel(viewModel);

            Assert.Empty(validationResult);
        }

        [Theory]
        [InlineData(CompetentAuthority.England, "invalid@email.com")]
        [InlineData(CompetentAuthority.NorthernIreland, "invalid@email.com")]
        [InlineData(CompetentAuthority.Scotland, "invalid@email.com")]
        [InlineData(CompetentAuthority.Wales, "invalid@email.com")]
        public void InvalidAgencyEmailAddresses(CompetentAuthority competentAuthority, string email)
        {
            viewModel.CompetentAuthority = competentAuthority;
            viewModel.Email = email;
            var validationResult = ViewModelValidator.ValidateViewModel(viewModel);

            Assert.Equal(1, validationResult.Count);
        }

        [Theory]
        [InlineData(CompetentAuthority.England, "invalid@email.com")]
        [InlineData(CompetentAuthority.NorthernIreland, "invalid@email.com")]
        [InlineData(CompetentAuthority.Scotland, "invalid@email.com")]
        [InlineData(CompetentAuthority.Wales, "invalid@email.com")]
        public void CanHaveInvalidAgencyEmailAddressesesWhenNotLiveEnvironment(CompetentAuthority competentAuthority,
            string email)
        {
            A.CallTo(() => dependencyResolver.GetService(typeof(AppConfiguration))).Returns(
                new AppConfiguration()
                {
                    Environment = "TEST"
                });

            viewModel.CompetentAuthority = competentAuthority;
            viewModel.Email = email;
            var validationResult = ViewModelValidator.ValidateViewModel(viewModel);

            Assert.Empty(validationResult);
        }

        private class TestViewModel
        {
            public CompetentAuthority? CompetentAuthority { get; set; }

            [CompetentAuthorityEmailAddress("CompetentAuthority")]
            public string Email { get; set; }
        }
    }
}