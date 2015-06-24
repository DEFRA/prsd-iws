namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.StateOfImport;
    using Core.Shared;
    using Core.StateOfImport;
    using Core.TransportRoute;
    using FakeItEasy;
    using Prsd.Core.Mapper;
    using Requests.StateOfImport;
    using Xunit;

    public class StateOfImportControllerTests
    {
        private readonly StateOfImportController controller;
        private readonly IIwsClient client;
        private static readonly Guid NullStateOfImportGuid = new Guid("6DF6FDA2-BA9F-477D-94D9-F1E03F4D2E61");
        private static readonly Guid ExistingStateOfImportGuid = new Guid("3B541912-2D24-4C7F-B6E5-20831363CE44");
        private static readonly Guid AnyCompetentAuthorityId = new Guid("92D4DF75-A6D1-413F-9C6C-B69DA69F006A");
        private static readonly Guid AnyCountryId = new Guid("96CB73BB-5F7A-4BBD-84B1-F720F2A0DB23");
        private static readonly Guid AnyEntryOrExitPointId = new Guid("68F189F6-BF5A-43D6-8F51-26D1DD637864");
        private const string AnyString = "test";

        public StateOfImportControllerTests()
        {
            client = A.Fake<IIwsClient>();

            var countries = new[]
            {
                new CountryData { Id = AnyCountryId, Name = AnyString }
            };

            A.CallTo(
                () => client.SendAsync(A<string>.Ignored, A<GetStateOfImportSetDataByNotificationId>.That.Matches(s => s.Id == NullStateOfImportGuid)))
                .Returns(new StateOfImportSetData
                {
                    Countries = countries
                });
            A.CallTo(
                () => client.SendAsync(A<string>.Ignored, A<GetStateOfImportSetDataByNotificationId>.That.Matches(s => s.Id == ExistingStateOfImportGuid)))
                .Returns(new StateOfImportSetData
                {
                    Countries = countries,
                    StateOfImport = new StateOfImportData
                {
                    CompetentAuthority = new CompetentAuthorityData
                    {
                        Id = AnyCompetentAuthorityId,
                        Name = AnyString
                    },
                    Country = new CountryData
                    {
                        Id = AnyCountryId,
                        Name = AnyString
                    },
                    EntryPoint = new EntryOrExitPointData
                    {
                        Id = AnyEntryOrExitPointId,
                        CountryId = AnyCountryId,
                        Name = AnyString
                    }
                }
                });

            this.controller = new StateOfImportController(() => client, new TestMap());
        }

        [Fact]
        public async Task Index_ReturnsCorrectViewModel()
        {
            var result = await controller.Index(ExistingStateOfImportGuid) as ViewResult;

            Assert.IsType<StateOfImportViewModel>(result.Model);

            var model = result.Model as StateOfImportViewModel;

            Assert.False(model.ShowNextSection);
        }

        private class TestMap : IMap<StateOfImportSetData, StateOfImportViewModel>
        {
            public StateOfImportViewModel Map(StateOfImportSetData source)
            {
                return new StateOfImportViewModel
                {
                    ShowNextSection = false,
                    Countries = new SelectList(new CountryData[]
                    {
                        new CountryData { Id = AnyCountryId, Name = AnyString }
                    }, "Id", "Name")
                };
            }
        }
    }
}
