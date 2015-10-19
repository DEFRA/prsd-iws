namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.WasteType;
    using Core.WasteType;
    using FakeItEasy;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.WasteType;
    using Web.ViewModels.Shared;
    using Xunit;

    public class WasteTypeControllerTests
    {
        private readonly IMediator mediator;
        private readonly WasteTypeController wasteTypeController;
        private readonly Guid notificationId = new Guid("D711F96B-3AF8-46BC-91B9-B906F764FF22");

        public WasteTypeControllerTests()
        {
            mediator = A.Fake<IMediator>();
            wasteTypeController = new WasteTypeController(mediator, new ChemicalCompositionAdditionalInformationMap());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public void ChemicalComposition_Post_BackToOverview_MaintainsRouteValue(bool? backToOverview)
        {
            var chemicalCompositionType = RadioButtonStringCollectionViewModel.CreateFromEnum<ChemicalCompositionType>();
            chemicalCompositionType.SelectedValue = "Wood";
            var model = new ChemicalCompositionViewModel()
            {
                ChemicalCompositionType = chemicalCompositionType
            };
            var result = wasteTypeController.ChemicalComposition(model, backToOverview) as RedirectToRouteResult;
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
            var result = await wasteTypeController.OtherWaste(model, backToOverview) as RedirectToRouteResult;
            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.Equal<bool?>(backToOverview.GetValueOrDefault(),
                ((bool?)result.RouteValues[backToOverviewKey]).GetValueOrDefault());
        }

        [Fact]
        public async Task OtherWasteAdditionalInformation_Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = new OtherWasteAdditionalInformationViewModel();
            var result = await wasteTypeController.OtherWasteAdditionalInformation(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task OtherWasteAdditionalInformation_Post_BackToOverviewFalse_RedirectsToWasteGenerationProcess()
        {
            var model = new OtherWasteAdditionalInformationViewModel();
            var result = await wasteTypeController.OtherWasteAdditionalInformation(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "WasteGenerationProcess");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public async Task WoodType_Post_BackToOverview_MaintainsRouteValue(bool? backToOverview)
        {
            var wasteComposition = new List<WasteTypeCompositionData>();
            var otherCodes = new List<WasteTypeCompositionData>();
            var model = new ChemicalCompositionConcentrationLevelsViewModel()
            {
                Command = string.Empty,
                ChemicalCompositionType = ChemicalCompositionType.Wood,
                WasteComposition = wasteComposition,
                OtherCodes = otherCodes,
                NotificationId = notificationId
            };
            var result = await wasteTypeController.WoodType(model, backToOverview) as RedirectToRouteResult;
            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.Equal<bool?>(backToOverview.GetValueOrDefault(),
                ((bool?)result.RouteValues[backToOverviewKey]).GetValueOrDefault());
        }

        [Fact]
        public async Task WoodAdditionalInformation_Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var wasteComposition = new List<WoodInformationData>();
            var model = new ChemicalCompositionInformationViewModel() 
            {
                NotificationId = notificationId,
                ChemicalCompositionType = ChemicalCompositionType.Wood,
                FurtherInformation = string.Empty,
                Energy = string.Empty,
                HasAnnex = true,
                WasteComposition = wasteComposition
            };
            var result = await wasteTypeController.WoodAdditionalInformation(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }
        
        [Fact]
        public async Task WoodAdditionalInformation_Post_BackToOverviewFalse_RedirectsToWasteGenerationProcess()
        {
            var wasteComposition = new List<WoodInformationData>();
            var model = new ChemicalCompositionInformationViewModel()
            {
                NotificationId = notificationId,
                ChemicalCompositionType = ChemicalCompositionType.Wood,
                FurtherInformation = string.Empty,
                Energy = string.Empty,
                HasAnnex = true,
                WasteComposition = wasteComposition
            };
            var result = await wasteTypeController.WoodAdditionalInformation(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "WasteGenerationProcess");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public async Task RdfSrfType_Post_BackToOverview_MaintainsRouteValue(bool? backToOverview)
        {
            var wasteComposition = new List<WasteTypeCompositionData>();
            var otherCodes = new List<WasteTypeCompositionData>();
            var model = new ChemicalCompositionConcentrationLevelsViewModel()
            {
                Command = string.Empty,
                ChemicalCompositionType = ChemicalCompositionType.RDF,
                WasteComposition = wasteComposition,
                OtherCodes = otherCodes,
                NotificationId = notificationId
            };
            var result = await wasteTypeController.RdfSrfType(model, backToOverview) as RedirectToRouteResult;
            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.Equal<bool?>(backToOverview.GetValueOrDefault(),
                ((bool?)result.RouteValues[backToOverviewKey]).GetValueOrDefault());
        }

        [Fact]
        public async Task RdfAdditionalInformation_Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var wasteComposition = new List<WoodInformationData>();
            var model = new ChemicalCompositionInformationViewModel()
            {
                NotificationId = notificationId,
                ChemicalCompositionType = ChemicalCompositionType.RDF,
                FurtherInformation = string.Empty,
                Energy = string.Empty,
                HasAnnex = true,
                WasteComposition = wasteComposition
            };
            var result = await wasteTypeController.RdfAdditionalInformation(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task RdfAdditionalInformation_Post_BackToOverviewFalse_RedirectsToWasteGenerationProcess()
        {
            var wasteComposition = new List<WoodInformationData>();
            var model = new ChemicalCompositionInformationViewModel()
            {
                NotificationId = notificationId,
                ChemicalCompositionType = ChemicalCompositionType.RDF,
                FurtherInformation = string.Empty,
                Energy = string.Empty,
                HasAnnex = true,
                WasteComposition = wasteComposition
            };
            var result = await wasteTypeController.RdfAdditionalInformation(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "WasteGenerationProcess");
        }
    }
}
