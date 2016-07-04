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
        [InlineData(UKCompetentAuthority.England, "valid@environment-agency.gov.uk")]
        [InlineData(UKCompetentAuthority.NorthernIreland, "valid@daera-ni.gov.uk")]
        [InlineData(UKCompetentAuthority.Scotland, "valid@sepa.org.uk")]
        [InlineData(UKCompetentAuthority.Wales, "valid@cyfoethnaturiolcymru.gov.uk")]
        [InlineData(UKCompetentAuthority.Wales, "valid@naturalresourceswales.gov.uk")]
        public void ValidAgencyEmailAddresses(UKCompetentAuthority competentAuthority, string email)
        {
            viewModel.CompetentAuthority = competentAuthority;
            viewModel.Email = email;
            var validationResult = ViewModelValidator.ValidateViewModel(viewModel);

            Assert.Empty(validationResult);
        }

        [Theory]
        [InlineData(UKCompetentAuthority.England, "invalid@email.com")]
        [InlineData(UKCompetentAuthority.NorthernIreland, "invalid@email.com")]
        [InlineData(UKCompetentAuthority.Scotland, "invalid@email.com")]
        [InlineData(UKCompetentAuthority.Wales, "invalid@email.com")]
        public void InvalidAgencyEmailAddresses(UKCompetentAuthority competentAuthority, string email)
        {
            viewModel.CompetentAuthority = competentAuthority;
            viewModel.Email = email;
            var validationResult = ViewModelValidator.ValidateViewModel(viewModel);

            Assert.Equal(1, validationResult.Count);
        }

        [Theory]
        [InlineData(UKCompetentAuthority.England, "invalid@email.com")]
        [InlineData(UKCompetentAuthority.NorthernIreland, "invalid@email.com")]
        [InlineData(UKCompetentAuthority.Scotland, "invalid@email.com")]
        [InlineData(UKCompetentAuthority.Wales, "invalid@email.com")]
        public void CanHaveInvalidAgencyEmailAddressesesWhenNotLiveEnvironment(UKCompetentAuthority competentAuthority,
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
            public UKCompetentAuthority? CompetentAuthority { get; set; }

            [CompetentAuthorityEmailAddress("CompetentAuthority")]
            public string Email { get; set; }
        }
    }
}