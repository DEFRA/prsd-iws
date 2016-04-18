namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using Domain.TransportRoute;
    using TestHelpers.Helpers;
    using Xunit;

    public class StateOfExportTests
    {
        [Fact]
        public void StateOfExport_AllCountriesMatch_ReturnsNewStateOfExport()
        {
            var country = GetTestCountry(Guid.Empty);

            var competentAuthority = ObjectInstantiator<CompetentAuthority>.CreateNew();
            ObjectInstantiator<CompetentAuthority>.SetProperty(x => x.Country, country, competentAuthority);

            var exitPoint = ObjectInstantiator<EntryOrExitPoint>.CreateNew();
            ObjectInstantiator<EntryOrExitPoint>.SetProperty(x => x.Country, country, exitPoint);

            var stateOfExport = new StateOfExport(country, competentAuthority, exitPoint);

            Assert.NotNull(stateOfExport);
        }

        [Theory]
        [InlineData("BE8189B9-CEAA-45EF-A920-10712FEEFCAA", "BE8189B9-CEAA-45EF-A920-10712FEEFCAA", "BE8189B9-CEAA-45EF-A920-10712FEEFCAB")]
        [InlineData("BE8189B9-CEAA-45EF-A920-10712FEEFCAA", "BE8189B9-CEAA-45EF-A920-10712FEEFCAB", "BE8189B9-CEAA-45EF-A920-10712FEEFCAB")]
        [InlineData("BE8189B9-CEAA-45EF-A920-10712FEEFCAA", "BE8189B9-CEAA-45EF-A920-10712FEEFCAB", "BE8189B9-CEAA-45EF-A920-10712FEEFCAA")]
        public void StateOfExport_CountriesDifferent_Throws(string countryId, string competentAuthorityCountryId, string exitCountryId)
        {
            var country = GetTestCountry(new Guid(countryId));

            var competentAuthority = ObjectInstantiator<CompetentAuthority>.CreateNew();
            ObjectInstantiator<CompetentAuthority>.SetProperty(x => x.Country, GetTestCountry(new Guid(competentAuthorityCountryId)), competentAuthority);

            var exitPoint = ObjectInstantiator<EntryOrExitPoint>.CreateNew();
            ObjectInstantiator<EntryOrExitPoint>.SetProperty(x => x.Country, GetTestCountry(new Guid(exitCountryId)), exitPoint);
            
            Assert.Throws<InvalidOperationException>(() => new StateOfExport(country, competentAuthority, exitPoint));
        }

        private Country GetTestCountry(Guid id)
        {
            var country = ObjectInstantiator<Country>.CreateNew();
            ObjectInstantiator<Country>.SetProperty(x => x.Id, id, country);

            return country;
        }
    }
}
