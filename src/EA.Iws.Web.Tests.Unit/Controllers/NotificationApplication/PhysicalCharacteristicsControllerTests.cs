namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using EA.Iws.Api.Client;
    using EA.Iws.Web.Areas.NotificationApplication.Controllers;
    using EA.Iws.Web.Areas.NotificationApplication.ViewModels.PhysicalCharacteristics;
    using EA.Iws.Web.ViewModels.Shared;
    using FakeItEasy;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class PhysicalCharacteristicsControllerTests
    {
        private readonly IIwsClient client;
        private readonly PhysicalCharacteristicsController physicalCharacteristicsController;

        public PhysicalCharacteristicsControllerTests()
        {
            client = A.Fake<IIwsClient>();
            physicalCharacteristicsController = new PhysicalCharacteristicsController(() => client);
        }

        [Fact]
        public async Task Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var physicalCharacteristics = new CheckBoxCollectionViewModel();
            physicalCharacteristics.PossibleValues = new[] { new SelectListItem() };
            var model = new PhysicalCharacteristicsViewModel()
            {
                PhysicalCharacteristics = physicalCharacteristics
            };
            var result = await physicalCharacteristicsController.Index(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task Post_BackToOverviewFalse_RedirectsToWasteCodes()
        {
            var physicalCharacteristics = new CheckBoxCollectionViewModel();
            physicalCharacteristics.PossibleValues = new[] { new SelectListItem() };
            var model = new PhysicalCharacteristicsViewModel()
            {
                PhysicalCharacteristics = physicalCharacteristics
            };
            var result = await physicalCharacteristicsController.Index(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "BaselOecdCode");
        }
    }
}
