﻿namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.ChemicalComposition;
    using Core.Notification.Audit;
    using Core.WasteType;
    using EA.Iws.Core.WasteComponentType;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.WasteType;
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
            chemicalCompositionController = new ChemicalCompositionController(mediator, this.auditService);

            A.CallTo(
                () =>
                    mediator.SendAsync(A<GetWasteType>.That.Matches(p => p.NotificationId == notificationId)))
                .Returns(new WasteTypeData()
                {
                    WasteCompositionData = new List<WasteCompositionData>()
                    {
                        A.Fake<WasteCompositionData>()
                    }
                });

            A.CallTo(
                () => mediator.SendAsync(A<GetNotificationAuditTable>.That.Matches
                (p => p.NotificationId == notificationId)))
                .Returns(CreateTestAuditTable());

            A.CallTo(() => auditService.AddAuditEntry(this.mediator, notificationId, "user", NotificationAuditType.Added, NotificationAuditScreenType.ChemicalComposition));
        }

        private NotificationAuditTable CreateTestAuditTable()
        {
            NotificationAuditTable table = new NotificationAuditTable();

            table.TableData = new List<NotificationAuditForDisplay>();
            table.TableData.Add(new NotificationAuditForDisplay(string.Empty, NotificationAuditScreenType.ChemicalComposition.ToString(), NotificationAuditType.Added.ToString(), DateTime.Now));

            return table;
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
        public async Task WasteCategory_ReturnsView()
        {
            var model = new WasteCategoryViewModel
            {
                NotificationId = notificationId,
                WasteCategoryType = RadioButtonStringCollectionViewModel.CreateFromEnum<WasteCategoryType>()
            };

            var result = await chemicalCompositionController.WasteCategory(notificationId) as ViewResult;
            var resultModel = (WasteCategoryViewModel)(result.Model);

            Assert.Equal(string.Empty, result.ViewName);
            Assert.Equal(model.NotificationId, resultModel.NotificationId);
            Assert.Equal(model.WasteCategoryType.PossibleValues.Count(), resultModel.WasteCategoryType.PossibleValues.Count());
            Assert.Equal(model.WasteCategoryType.SelectedValue, resultModel.WasteCategoryType.SelectedValue);
        }

        [Fact]
        public void OtherWaste_Post_RedirectedTo_BackToWasteCategory()
        {
            var chemicalCompositionType = RadioButtonStringCollectionViewModel.CreateFromEnum<ChemicalComposition>();
            chemicalCompositionType.SelectedValue = ChemicalComposition.Other.ToString();
            var model = new ChemicalCompositionTypeViewModel()
            {
                ChemicalCompositionType = chemicalCompositionType
            };
            var result = chemicalCompositionController.Index(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "WasteCategory", "ChemicalComposition");
        }

        [Fact]
        public async Task OtherWaste_WasteCategory_RedirectedTo_WasteComponent()
        {
            var wasteCategoryType = RadioButtonStringCollectionViewModel.CreateFromEnum<WasteCategoryType>();
            wasteCategoryType.SelectedValue = WasteCategoryType.Batteries.ToString();

            var wasteCategoryViewModel = new WasteCategoryViewModel()
            {
                WasteCategoryType = wasteCategoryType
            };
            var result = await chemicalCompositionController.WasteCategory(wasteCategoryViewModel, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "WasteComponent", "ChemicalComposition");
        }

        [Fact]
        public async Task WasteComponent_ReturnsView()
        {
            var model = new WasteComponentViewModel
            {
                NotificationId = notificationId,
                WasteComponentTypes = CheckBoxCollectionViewModel.CreateFromEnum<WasteComponentType>()
            };

            var result = await chemicalCompositionController.WasteComponent(notificationId) as ViewResult;
            var resultModel = (WasteComponentViewModel)(result.Model);

            Assert.Equal(string.Empty, result.ViewName);
            Assert.Equal(model.NotificationId, resultModel.NotificationId);
            Assert.Equal(model.WasteComponentTypes.PossibleValues.Count(), resultModel.WasteComponentTypes.PossibleValues.Count());
        }

        [Fact]
        public async Task OtherWaste_WasteComponent_RedirectedTo_OtherWaste()
        {
            var wasteComponentType = CheckBoxCollectionViewModel.CreateFromEnum<WasteComponentType>();
            var selectedValues = new[] { 1, 2 };
            wasteComponentType.SetSelectedValues(selectedValues);

            var wasteComponentViewModel = new WasteComponentViewModel()
            {
                WasteComponentTypes = wasteComponentType
            };
            var result = await chemicalCompositionController.WasteComponent(wasteComponentViewModel, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "OtherWaste", "ChemicalComposition");
        }

        [Fact]
        public async Task OtherWaste_ReturnsView()
        {
            var model = new OtherWasteViewModel()
            {
                NotificationId = notificationId,
                Description = "Test Description"
            };

            var result = await chemicalCompositionController.OtherWaste(notificationId) as ViewResult;
            var resultModel = (OtherWasteViewModel)result.Model;

            Assert.Equal(string.Empty, result.ViewName);
            Assert.Equal(model.NotificationId, resultModel.NotificationId);
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
