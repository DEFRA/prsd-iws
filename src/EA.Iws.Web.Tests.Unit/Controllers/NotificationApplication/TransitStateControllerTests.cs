namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.TransitState;
    using Core.Notification.Audit;
    using Core.Shared;
    using Core.TransportRoute;
    using FakeItEasy;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.TransitState;
    using Requests.TransportRoute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.Infrastructure;
    using Xunit;

    public class TransitStateControllerTests
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;
        private readonly TransitStateController transitStateController;
        private const string UnitedKingdom = "United Kingdom";
        private const string anyString = "test";
        private static readonly Guid notificationId = new Guid();
        private static readonly Guid UnitedKingdomGuid = new Guid("87CC7C67-05B5-469B-83FF-D936C597F2B0");
        private static readonly Guid HollandGuid = new Guid("87CC7C67-05B5-469B-83FF-D936C597F2B0");
        private static readonly Guid JerseyGuid = new Guid("60365C62-967E-406E-8201-1A9C895D78A3");
        private static readonly Guid GuernseyGuid = new Guid("87A1DB30-DF34-4BB1-8C84-0E51FE8C4B05");
        private static readonly Guid IsleOfManGuid = new Guid("671804E9-F2EE-41A6-9624-D8B3AAD73F2F");

        public TransitStateControllerTests()
        {
            mediator = A.Fake<IMediator>();
            this.auditService = A.Fake<IAuditService>();
            var competentAuthorties = new[] 
                {
                    new CompetentAuthorityData { Id = environmentAgency.Id, Name = anyString }
                };
            var entryOrExitPoints = new[]
                {
                    new EntryOrExitPointData { Id = hull.Id, CountryId = hull.CountryId, Name = anyString }
                };
            A.CallTo(
                () => mediator.SendAsync(A<GetTransitAuthoritiesAndEntryOrExitPointsByCountryId>.That.Matches(s => s.Id == hull.CountryId)))
                .Returns(new CompetentAuthorityAndEntryOrExitPointData()
                {
                    CompetentAuthorities = competentAuthorties,
                    EntryOrExitPoints = entryOrExitPoints
                });
            transitStateController = new TransitStateController(mediator, new TestMap(), this.auditService);
            A.CallTo(() => auditService.AddAuditEntry(this.mediator, notificationId, "user", NotificationAuditType.Create, "screen"));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public async Task Index_Post_BackToOverview_MaintainsRouteValue(bool? backToOverview)
        {
            var model = new TransitStateViewModel()
            {
                CountryId = hull.CountryId,
                ExitPointId = hull.Id,
                EntryPointId = europoort.Id
            };
            var result = await transitStateController.Index(notificationId, null, model, anyString, backToOverview) as RedirectToRouteResult;
            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.Equal<bool>(backToOverview.GetValueOrDefault(),
                ((bool?)result.RouteValues[backToOverviewKey]).GetValueOrDefault());
        }

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

        private static List<CountryData> GetCountryData()
        {
            return new[]
            {
                new CountryData { Id = UnitedKingdomGuid, Name = UnitedKingdom },
                new CountryData { Id = HollandGuid, Name = "Holland" },
                new CountryData { Id = JerseyGuid, Name = "Jersey" },
                new CountryData { Id = GuernseyGuid, Name = "Guernsey" },
                new CountryData { Id = IsleOfManGuid, Name = "Isle of Man" }
            }.ToList();
        }
        
        private class TestMap : IMap<TransitStateWithTransportRouteData, TransitStateViewModel>
        {
            public TransitStateViewModel Map(TransitStateWithTransportRouteData source)
            {
                return new TransitStateViewModel
                {
                    ShowNextSection = false,
                    Countries = new SelectList(GetCountryData(), "Id", "Name")
                };
            }
        }
    }
}
