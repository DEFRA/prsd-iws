namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.WasteOperations;
    using Core.OperationCodes;
    using Core.TechnologyEmployed;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.TechnologyEmployed;
    using Xunit;

    public class WasteOperationsControllerTests
    {
        private readonly Guid notificationId = Guid.NewGuid();
        private readonly WasteOperationsController wasteOperationsController;

        public WasteOperationsControllerTests()
        {
            var mediator = A.Fake<IMediator>();
            wasteOperationsController = new WasteOperationsController(mediator);

            A.CallTo(() => mediator.SendAsync(A<GetTechnologyEmployed>.Ignored))
                .Returns(new TechnologyEmployedData
                {
                    OperationCodes = new List<OperationCode>()
                });

            A.CallTo(() => mediator.SendAsync(A<SetTechnologyEmployed>.Ignored))
                .Returns(Guid.Empty);
        }

        [Fact]
        public async Task TechnologyEmployed_GetByNotificationId_RedirectsTo_View()
        {
            var result = await wasteOperationsController.TechnologyEmployed(notificationId) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Invalid_TechnologyEmployed_Returns_Same_View()
        {
            var model = new TechnologyEmployedViewModel();
            wasteOperationsController.ModelState.AddModelError("Test", "Error");

            var result = await wasteOperationsController.TechnologyEmployed(model) as ViewResult;
            Assert.Equal(string.Empty, result.ViewName);
        }
    }
}
