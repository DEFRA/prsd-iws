namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using EA.Iws.Api.Client;
    using EA.Iws.Core.Shared;
    using EA.Iws.Core.TransportRoute;
    using EA.Iws.Requests.StateOfExport;
    using EA.Iws.Requests.TransportRoute;
    using EA.Iws.Web.Areas.NotificationApplication.Controllers;
    using EA.Iws.Web.Areas.NotificationApplication.ViewModels.StateOfExport;
    using EA.Prsd.Core.Mapper;
    using FakeItEasy;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class StateOfExportControllerTests
    {
        private readonly IIwsClient client;
        private static readonly Guid anyCountryId = new Guid("25D2D146-942E-46DE-926E-00E2ECFB45C7");
        private static readonly Guid anyCompetentAuthorityId = new Guid("1BF015B8-56C6-43C2-BB8B-3A3FF39135BC");
        private static readonly Guid anyEntryOrExitPointId = new Guid("72F68B61-E969-42DB-AF7F-E0B9FEDB7BBF");
        private static readonly Guid notificationId = new Guid("DCE0C3C1-4B1E-4741-8480-AB7645027646");
        private static readonly string anyString = "test";
        private readonly StateOfExportController stateOfExportController;

        public StateOfExportControllerTests()
        {
            client = A.Fake<IIwsClient>();
            var competentAuthorties = new[] 
                {
                    new CompetentAuthorityData { Id = anyCompetentAuthorityId, Name = anyString }
                };
            var entryOrExitPoints = new[]
                {
                    new EntryOrExitPointData { Id = anyEntryOrExitPointId, CountryId = anyCountryId, Name = anyString }
                };
            A.CallTo(
                () => client.SendAsync(A<string>.Ignored,
                    A<GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId>.That.Matches(s => s.Id == anyCountryId)))
                .Returns(new CompententAuthorityAndEntryOrExitPointData()
                {
                    CompetentAuthorities = competentAuthorties,
                    EntryOrExitPoints = entryOrExitPoints
                });
            stateOfExportController = new StateOfExportController(() => client, new TestMap());
        }

        [Fact]
        public async Task Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = CreateStateOfExportViewModel();
            var result = await stateOfExportController.Index(notificationId, model, anyString, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task Post_BackToOverviewFalse_RedirectsToStateOfImport()
        {
            var model = CreateStateOfExportViewModel();
            var result = await stateOfExportController.Index(notificationId, model, anyString, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "StateOfImport");
        }

        private StateOfExportViewModel CreateStateOfExportViewModel()
        {
            return new StateOfExportViewModel()
            {
                CountryId = anyCountryId,
                EntryOrExitPointId = anyEntryOrExitPointId,
            };
        }

        private class TestMap : IMap<StateOfExportWithTransportRouteData, StateOfExportViewModel>
        {
            public StateOfExportViewModel Map(StateOfExportWithTransportRouteData source)
            {
                return new StateOfExportViewModel();
            }
        }
    }
}
