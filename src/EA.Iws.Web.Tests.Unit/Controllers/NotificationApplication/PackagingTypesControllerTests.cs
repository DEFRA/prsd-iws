namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using EA.Iws.Api.Client;
    using EA.Iws.Core.PackagingType;
    using EA.Iws.Web.Areas.NotificationApplication.Controllers;
    using EA.Iws.Web.Areas.NotificationApplication.ViewModels.PackagingTypes;
    using EA.Iws.Web.ViewModels.Shared;
    using FakeItEasy;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class PackagingTypesControllerTests
    {
        private readonly IIwsClient client;
        private readonly Guid notificationId = new Guid("DD1F019D-BD85-4A6F-89AB-328A7BD53CEA");
        private readonly PackagingTypesController packagingTypesController;

        public PackagingTypesControllerTests()
        {
            client = A.Fake<IIwsClient>();
            packagingTypesController = new PackagingTypesController(() => client);
        }

        private PackagingTypesViewModel CreateValidPackagingTypesViewModel()
        {
            var packagingTypes = CheckBoxCollectionViewModel.CreateFromEnum<PackagingType>();
            var selectedValues = new[] { 1, 2 };
            packagingTypes.SetSelectedValues(selectedValues);
            return new PackagingTypesViewModel()
            {
                PackagingTypes = packagingTypes,
                NotificationId = notificationId
            };
        }

        [Fact]
        public async Task Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = CreateValidPackagingTypesViewModel();
            var result = await packagingTypesController.Index(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task Post_BackToOverviewFalse_RedirectsToSpecialHandling()
        {
            var model = CreateValidPackagingTypesViewModel();
            var result = await packagingTypesController.Index(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "SpecialHandling");
        }
    }
}
