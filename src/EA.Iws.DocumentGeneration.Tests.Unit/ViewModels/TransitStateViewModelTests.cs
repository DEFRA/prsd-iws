namespace EA.Iws.DocumentGeneration.Tests.Unit.ViewModels
{
    using System;
    using System.Collections.Generic;
    using DocumentGeneration.ViewModels;
    using Domain;
    using Domain.TransportRoute;
    using TestHelpers.Helpers;
    using Xunit;

    public class TransitStateViewModelTests
    {
        [Fact]
        public void CanCreateTransitStateViewModelTest()
        {
            var vm = new TransitStateViewModel();

            Assert.IsType(typeof(TransitStateViewModel), vm);
        }

        [Fact]
        public void OneStateDisplaysInMiddleOnFormTest()
        {
            var statesList = new List<TransitState>();
            statesList.Add(TransitState(CountryFactory.Create(Guid.Empty, "one"), "short name"));

            var vm = new TransitStateViewModel(statesList);

            Assert.Equal(string.Empty, vm.AnnexMessage);
            Assert.Equal("one", vm.MiddleCountry);
            Assert.Equal(string.Empty, vm.LeftCountry);
            Assert.Equal(string.Empty, vm.RightCountry);
        }

        [Fact]
        public void OneStateDisplaysInAnnexIfPointNameLengthGreaterThan12Test()
        {
            var statesList = new List<TransitState>();
            statesList.Add(TransitState(CountryFactory.Create(Guid.Empty, "one"), "long name >12"));

            var vm = new TransitStateViewModel(statesList);
            vm.SetAnnexMessage(6);

            Assert.Equal("See Annex 6", vm.AnnexMessage);
            Assert.Equal(string.Empty, vm.MiddleCountry);
            Assert.Equal(string.Empty, vm.LeftCountry);
            Assert.Equal(string.Empty, vm.RightCountry);
            Assert.Equal(1, vm.TransitStateDetails.Count);
            Assert.Equal("one", vm.TransitStateDetails[0].Country);
        }

        [Fact]
        public void TwoStatesDisplaysOnLeftAndRightOnFormTest()
        {
            var statesList = new List<TransitState>();
            statesList.Add(TransitState(CountryFactory.Create(Guid.Empty, "one"), "short name"));
            statesList.Add(TransitState(CountryFactory.Create(Guid.Empty, "two"), "short name"));

            var vm = new TransitStateViewModel(statesList);

            Assert.Equal(string.Empty, vm.AnnexMessage);
            Assert.Equal(string.Empty, vm.MiddleCountry);
            Assert.Equal("one", vm.LeftCountry);
            Assert.Equal("two", vm.RightCountry);
        }

        [Fact]
        public void ThreeStatesDisplaysOnFormTest()
        {
            var statesList = new List<TransitState>();
            statesList.Add(TransitState(CountryFactory.Create(Guid.Empty, "one"), "short name"));
            statesList.Add(TransitState(CountryFactory.Create(Guid.Empty, "two"), "short name"));
            statesList.Add(TransitState(CountryFactory.Create(Guid.Empty, "three"), "short name"));

            var vm = new TransitStateViewModel(statesList);

            Assert.Equal(string.Empty, vm.AnnexMessage);
            Assert.Equal("two", vm.MiddleCountry);
            Assert.Equal("one", vm.LeftCountry);
            Assert.Equal("three", vm.RightCountry);
        }

        [Fact]
        public void MoreThanThreeStatesDisplaysInAnnexTest()
        {
            var statesList = new List<TransitState>();
            statesList.Add(TransitState(CountryFactory.Create(Guid.Empty, "one"), "short name"));
            statesList.Add(TransitState(CountryFactory.Create(Guid.Empty, "two"), "short name"));
            statesList.Add(TransitState(CountryFactory.Create(Guid.Empty, "three"), "short name"));
            statesList.Add(TransitState(CountryFactory.Create(Guid.Empty, "four"), "short name"));

            var vm = new TransitStateViewModel(statesList);
            vm.SetAnnexMessage(6);

            Assert.Equal("See Annex 6", vm.AnnexMessage);
            Assert.Equal(string.Empty, vm.MiddleCountry);
            Assert.Equal(string.Empty, vm.LeftCountry);
            Assert.Equal(string.Empty, vm.RightCountry);
            Assert.Equal(4, vm.TransitStateDetails.Count);
            Assert.Equal("one", vm.TransitStateDetails[0].Country);
            Assert.Equal("two", vm.TransitStateDetails[1].Country);
            Assert.Equal("three", vm.TransitStateDetails[2].Country);
            Assert.Equal("four", vm.TransitStateDetails[3].Country);
        }

        [Fact]
        public void SetAnnexMessageTest()
        {
            var statesList = new List<TransitState>();
            statesList.Add(TransitState(CountryFactory.Create(Guid.Empty, "one"), "short name"));
            var vm = new TransitStateViewModel(statesList);

            vm.SetAnnexMessage(6);

            Assert.Equal("See Annex 6", vm.AnnexMessage);
        }

        private static TransitState TransitState(Country country, string name)
        {
            var ca = CompetentAuthorityFactory.Create(Guid.Empty, country);
            var state = new TransitState(country, ca, EntryOrExitPointFactory.Create(Guid.Empty, country, name), EntryOrExitPointFactory.Create(Guid.NewGuid(), country, name), 1);
            return state;
        }
    }
}
