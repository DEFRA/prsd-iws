namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.ChemicalComposition;
    using Core.Notification.Audit;
    using Core.WasteType;
    using FakeItEasy;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.WasteType;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.Infrastructure;
    using Web.ViewModels.Shared;
    using Xunit;

    public class ChemicalCompositionControllerTests
    {
        private readonly IMediator mediator;
        private readonly ChemicalCompositionController chemicalCompositionController;
        private readonly Guid notificationId = new Guid("D711F96B-3AF8-46BC-91B9-B906F764FF22");
        private readonly IAuditService auditService;

        public ChemicalCompositionControllerTests()
        {
            mediator = A.Fake<IMediator>();
            this.auditService = A.Fake<IAuditService>();
            chemicalCompositionController = new ChemicalCompositionController(mediator, new ChemicalCompositionMap(), this.auditService);
            A.CallTo(() => auditService.AddAuditEntry(this.mediator, notificationId, "user", NotificationAuditType.Create, "screen"));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public void ChemicalComposition_Post_BackToOverview_MaintainsRouteValue(bool? backToOverview)
        {
            var chemicalCompositionType = RadioButtonStringCollectionViewModel.CreateFromEnum<ChemicalComposition>();
            chemicalCompositionType.SelectedValue = "Wood";
            var model = new ChemicalCompositionTypeViewModel()
            {
                ChemicalCompositionType = chemicalCompositionType
            };
            var result = chemicalCompositionController.Index(model, backToOverview) as RedirectToRouteResult;
            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.Equal<bool?>(backToOverview.GetValueOrDefault(),
                ((bool?)result.RouteValues[backToOverviewKey]).GetValueOrDefault());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public async Task OtherWaste_Post_BackToOverview_MaintainsRouteValue(bool? backToOverview)
        {
            var model = new OtherWasteViewModel();
            var result = await chemicalCompositionController.OtherWaste(model, backToOverview) as RedirectToRouteResult;
            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.Equal<bool?>(backToOverview.GetValueOrDefault(),
                ((bool?)result.RouteValues[backToOverviewKey]).GetValueOrDefault());
        }

        [Fact]
        public async Task OtherWasteAdditionalInformation_Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = new OtherWasteAdditionalInformationViewModel();
            var result = await chemicalCompositionController.OtherWasteAdditionalInformation(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task OtherWasteAdditionalInformation_Post_BackToOverviewFalse_RedirectsToWasteGenerationProcess()
        {
            var model = new OtherWasteAdditionalInformationViewModel();
            var result = await chemicalCompositionController.OtherWasteAdditionalInformation(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "WasteGenerationProcess");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public async Task Wood_Post_BackToOverview_MaintainsRouteValue(bool? backToOverview)
        {
            var wasteComposition = new List<WoodInformationData>();
            var model = new ChemicalCompositionViewModel()
            {
                ChemicalCompositionType = ChemicalComposition.Wood,
                WasteComposition = wasteComposition,
                NotificationId = notificationId
            };
            var result = await chemicalCompositionController.Parameters(model, backToOverview) as RedirectToRouteResult;
            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.Equal<bool?>(backToOverview.GetValueOrDefault(),
                ((bool?)result.RouteValues[backToOverviewKey]).GetValueOrDefault());
        }

        [Fact]
        public async Task WoodContined_Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var wasteComposition = new List<WasteTypeCompositionData>();
            var model = new ChemicalCompositionContinuedViewModel() 
            {
                NotificationId = notificationId,
                ChemicalCompositionType = ChemicalComposition.Wood,
                WasteComposition = wasteComposition,
                OtherCodes = wasteComposition,
                Command = "continue"
            };
            var result = await chemicalCompositionController.Constituents(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }
        
        [Fact]
        public async Task Wood_Post_BackToOverviewFalse_RedirectsToWasteGenerationProcess()
        {
            var wasteComposition = new List<WasteTypeCompositionData>();
            var model = new ChemicalCompositionContinuedViewModel()
            {
                NotificationId = notificationId,
                ChemicalCompositionType = ChemicalComposition.Wood,
                WasteComposition = wasteComposition,
                OtherCodes = wasteComposition,
                Command = "continue"
            };
            var result = await chemicalCompositionController.Constituents(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "WasteGenerationProcess");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public async Task RdfSrf_Post_BackToOverview_MaintainsRouteValue(bool? backToOverview)
        {
            var wasteComposition = new List<WoodInformationData>();
            var model = new ChemicalCompositionViewModel()
            {
                ChemicalCompositionType = ChemicalComposition.RDF,
                WasteComposition = wasteComposition,
                NotificationId = notificationId
            };
            var result = await chemicalCompositionController.Parameters(model, backToOverview) as RedirectToRouteResult;
            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.Equal<bool?>(backToOverview.GetValueOrDefault(),
                ((bool?)result.RouteValues[backToOverviewKey]).GetValueOrDefault());
        }

        [Fact]
        public async Task RdfSrfContinued_Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var wasteComposition = new List<WasteTypeCompositionData>();
            var model = new ChemicalCompositionContinuedViewModel
            {
                NotificationId = notificationId,
                ChemicalCompositionType = ChemicalComposition.RDF,
                WasteComposition = wasteComposition,
                OtherCodes = wasteComposition,
                Command = "continue"
            };
            var result = await chemicalCompositionController.Constituents(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task RdfSrf_Post_BackToOverviewFalse_RedirectsToWasteGenerationProcess()
        {
            var wasteComposition = new List<WasteTypeCompositionData>();
            var model = new ChemicalCompositionContinuedViewModel()
            {
                NotificationId = notificationId,
                ChemicalCompositionType = ChemicalComposition.RDF,
                WasteComposition = wasteComposition,
                OtherCodes = wasteComposition,
                Command = "continue"
            };
            var result = await chemicalCompositionController.Constituents(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "WasteGenerationProcess");
        }
    }
}
