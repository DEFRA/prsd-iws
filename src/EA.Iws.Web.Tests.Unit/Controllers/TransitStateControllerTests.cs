namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using FakeItEasy;
    using Requests.Registration;
    using Requests.Shared;
    using Requests.StateOfExport;
    using Requests.TransitState;
    using Requests.TransportRoute;
    using ViewModels.Shared;
    using ViewModels.TransitState;
    using Web.Controllers;
    using Xunit;

    public class TransitStateControllerTests
    {
        private const string UnitedKingdom = "United Kingdom";
        private static readonly Guid UnitedKingdomGuid = new Guid("87CC7C67-05B5-469B-83FF-D936C597F2B0");
        private static readonly Guid HollandGuid = new Guid("87CC7C67-05B5-469B-83FF-D936C597F2B0");
        private static readonly Guid JerseyGuid = new Guid("60365C62-967E-406E-8201-1A9C895D78A3");
        private static readonly Guid GuernseyGuid = new Guid("87A1DB30-DF34-4BB1-8C84-0E51FE8C4B05");
        private static readonly Guid IsleOfManGuid = new Guid("671804E9-F2EE-41A6-9624-D8B3AAD73F2F");

        private readonly CompetentAuthorityData environmentAgency = new CompetentAuthorityData
        {
            Name = "EA",
            CountryId = UnitedKingdomGuid,
            Id = new Guid("48276620-76FC-44FA-9E7A-D17CCD10A97E"),
            IsSystemUser = true,
            Abbreviation = "EA",
            Code = "GB01"
        };

        private readonly CompetentAuthorityData hollandAgency = new CompetentAuthorityData
        {
            Name = "Holland Environmental Protection",
            CountryId = HollandGuid,
            Id = new Guid("CFB47295-2BBA-414A-84CB-7D5E9D42D1F2"),
            IsSystemUser = false,
            Abbreviation = "EAH",
            Code = "H1"
        };

        private readonly EntryOrExitPointData europoort = new EntryOrExitPointData
        {
            CountryId = HollandGuid,
            Id = new Guid("9B954711-D0AE-4FF7-B0F3-5DB88EC05AF4"),
            Name = "Europoort"
        };

        private readonly EntryOrExitPointData harlingen = new EntryOrExitPointData
        {
            CountryId = HollandGuid,
            Id = new Guid("945DABCE-AE7B-46DF-9472-E8D86A337245"),
            Name = "Harlingen"
        };

        private readonly EntryOrExitPointData dover = new EntryOrExitPointData
        {
            CountryId = UnitedKingdomGuid,
            Id = new Guid("A5BC7FB7-E4B8-4891-B195-114B6D77CE96"),
            Name = "Dover"
        };

        private readonly EntryOrExitPointData hull = new EntryOrExitPointData
        {
            CountryId = UnitedKingdomGuid,
            Id = new Guid("BE672385-02B9-48B6-A211-5F6A1610EA1F"),
            Name = "Hull"
        };

        [Fact]
        public async Task Add_HttpGet_ReturnsModel()
        {
            var client = A.Fake<IIwsClient>();
            A.CallTo(() => client.SendAsync(A<GetCountries>._)).Returns(GetCountryData());

            A.CallTo(() => client.SendAsync(A<string>._, A<GetTransportRouteSummaryForNotification>._))
                .Returns(new TransportRouteData
                {
                    StateOfExportData = new StateOfExportData
                    {
                        ExitPoint = dover,
                        Country = new CountryData
                        {
                            Id = UnitedKingdomGuid,
                            Name = UnitedKingdom
                        },
                        CompetentAuthority = environmentAgency
                    }
                });

            var controller = new TransitStateController(() => client);

            var result = (await controller.Add(new Guid("F829E4B1-7F8B-44FC-9BBE-CEFB6A463B45"))) as ViewResult;

            Assert.IsType<TransitStateViewModel>(result.Model);
            var model = result.Model as TransitStateViewModel;

            Assert.Equal(UnitedKingdomGuid, model.StateOfExportCountryId);
            Assert.False(model.IsCountrySelected);
        }

        [Fact]
        public async Task Add_HttpPost_SelectCountryPostbackToView()
        {
            var client = A.Fake<IIwsClient>();
            A.CallTo(() => client.SendAsync(A<GetCountries>._)).Returns(GetCountryData());
            A.CallTo(() => client.SendAsync(A<GetCompetentAuthoritiesByCountry>._)).Returns(new[]
            {
                hollandAgency
            });
            A.CallTo(() => client.SendAsync(A<GetEntryOrExitPointsByCountry>._)).Returns(new[]            
            {
                europoort
            });

            var controller = new TransitStateController(() => client);

            var result = (await controller.Add(Guid.Empty, new TransitStateViewModel
            {
                IsCountrySelected = false,
                CountryId = HollandGuid,
                StateOfExportCountryId = UnitedKingdomGuid
            })) as ViewResult;
            
            Assert.IsType<TransitStateViewModel>(result.Model);

            var model = result.Model as TransitStateViewModel;

            Assert.True(model.IsCountrySelected);
            Assert.Equal(hollandAgency.Id, model.CompetentAuthorities.PossibleValues.First().Value);
        }

        [Fact]
        public async Task Add_HttpPost_SelectCountryWithErrorsPostbackToOriginalView()
        {
            var client = A.Fake<IIwsClient>();
            A.CallTo(() => client.SendAsync(A<GetCountries>._)).Returns(GetCountryData());

            var controller = new TransitStateController(() => client);

            controller.ModelState.AddModelError("CountryId", "A error occurred.");

            var result = await controller.Add(Guid.Empty, new TransitStateViewModel
            {
                IsCountrySelected = false,
                CountryId = UnitedKingdomGuid,
                StateOfExportCountryId = UnitedKingdomGuid
            }) as ViewResult;

            Assert.IsType<TransitStateViewModel>(result.Model);
            var model = result.Model as TransitStateViewModel;

            Assert.False(model.IsCountrySelected);
            Assert.Equal(UnitedKingdomGuid, model.CountryId);
        }

        [Fact]
        public async Task Add_HttpPost_FullSubmitRedirectsToSummary()
        {
            var client = A.Fake<IIwsClient>();
            A.CallTo(() => client.SendAsync(A<GetCountries>._)).Returns(GetCountryData());

            A.CallTo(() => client.SendAsync(A<GetEntryOrExitPointsByCountry>._)).Returns(new[]
            {
                europoort
            });

            A.CallTo(() => client.SendAsync(A<AddTransitStateToNotification>._)).Returns(Guid.Empty);

            var controller = new TransitStateController(() => client);

            var result = await controller.Add(Guid.Empty, new TransitStateViewModel
            {
                CountryId = HollandGuid,
                ExitPointId = europoort.Id,
                EntryPointId = harlingen.Id,
                IsCountrySelected = true,
                CompetentAuthorities = new StringGuidRadioButtons()
            }) as RedirectToRouteResult;

            Assert.Equal("Summary", result.RouteValues["action"]);
        }

        [Fact]
        public async Task Add_HttpPost_ErrorOnFullSubmit()
        {
            var client = A.Fake<IIwsClient>();
            A.CallTo(() => client.SendAsync(A<GetCountries>._)).Returns(GetCountryData());
            A.CallTo(() => client.SendAsync(A<GetEntryOrExitPointsByCountry>._)).Returns(new[]
            {
                europoort,
                harlingen
            });

            var controller = new TransitStateController(() => client);

            controller.ModelState.AddModelError("ExitPointId", "An error occurred.");

            var result = await controller.Add(Guid.Empty, new TransitStateViewModel
            {
                CountryId = HollandGuid,
                CompetentAuthorities = new StringGuidRadioButtons(),
                EntryPointId = europoort.Id,
                ExitPointId = harlingen.Id,
                IsCountrySelected = true
            }) as ViewResult;

            Assert.IsType<TransitStateViewModel>(result.Model);
            var model = result.Model as TransitStateViewModel;
            Assert.Equal(HollandGuid, model.CountryId);
            Assert.Equal(europoort.Id, model.EntryPointId);
        }

        private CountryData[] GetCountryData()
        {
            return new[]
            {
                new CountryData { Id = UnitedKingdomGuid, Name = UnitedKingdom },
                new CountryData { Id = HollandGuid, Name = "Holland" },
                new CountryData { Id = JerseyGuid, Name = "Jersey" },
                new CountryData { Id = GuernseyGuid, Name = "Guernsey" },
                new CountryData { Id = IsleOfManGuid, Name = "Isle of Man" }
            };
        }
    }
}
