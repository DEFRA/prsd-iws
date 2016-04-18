namespace EA.Iws.Web.Tests.Unit.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Areas.NotificationApplication.ViewModels.CustomsOffice;
    using TestHelpers;
    using Xunit;

    public class CustomsOfficeViewModelTests
    {
        private const string AnyName = "name";
        private const string AnyAddress = "address";
        private readonly CustomsOfficeViewModel model;

        public CustomsOfficeViewModelTests()
        {
            this.model = new CustomsOfficeViewModel
            {
                Address = AnyAddress,
                Name = AnyName,
                SelectedCountry = Guid.Empty
            };
        }

        [Fact]
        public void SelectedCountryIsNull_ReturnsError()
        {
            model.SelectedCountry = null;

            List<ValidationResult> result = ViewModelValidator.ValidateViewModel(model);

            Assert.Equal(1, result.Count);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void NameIsNullOrEmpty_ReturnsError(string name)
        {
            model.Name = null;

            List<ValidationResult> result = ViewModelValidator.ValidateViewModel(model);

            Assert.Equal(1, result.Count);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddressIsNullOrEmpty_ReturnsError(string address)
        {
            model.Address = address;

            List<ValidationResult> result = ViewModelValidator.ValidateViewModel(model);

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void NameTooLong_ReturnsError()
        {
            model.Name = new string('a', 1025);

            List<ValidationResult> result = ViewModelValidator.ValidateViewModel(model);

            Assert.Equal(1, result.Count);
        }
    }
}
