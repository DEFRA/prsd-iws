namespace EA.Iws.Web.Tests.Unit.Controllers.MovementDocument
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.MovementDocument.Controllers;
    using Areas.MovementDocument.ViewModels.NumberOfPackages;
    using FakeItEasy;
    using Requests.Movement;
    using Xunit;

    public class NumberOfPackagesControllerTests
    {
        private static readonly Guid AnyGuid = new Guid("C95DD463-ACF8-4B4E-B060-E6FDABBBD975");

        private readonly NumberOfPackagesController controller;
        private readonly IIwsClient client;

        public NumberOfPackagesControllerTests()
        {
            client = A.Fake<IIwsClient>();

            controller = new NumberOfPackagesController(() => client);
        }

        [Fact]
        public async Task GetReturnsNull_ReturnsNullInViewModel()
        {
            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<GetNumberOfPackagesByMovementId>.Ignored))
                .Returns<int?>(null);

            var result = await controller.Index(AnyGuid) as ViewResult;

            var model = result.Model as NumberOfPackagesViewModel;

            Assert.Equal(null, model.Number);
        }

        [Fact]
        public async Task GetReturnsNumber_ReturnsNumberInViewModel()
        {
            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<GetNumberOfPackagesByMovementId>.Ignored))
                .Returns(7);

            var result = await controller.Index(AnyGuid) as ViewResult;

            var model = result.Model as NumberOfPackagesViewModel;

            Assert.Equal(7, model.Number);
        }

        [Fact]
        public async Task GetSendsCorrectRequest()
        {
            await controller.Index(AnyGuid);

            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<GetNumberOfPackagesByMovementId>.That.Matches(r => r.Id == AnyGuid)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task PostModelStateInvalid_ReturnsView()
        {
            controller.ModelState.AddModelError("Error", "Error");

            var result = await controller.Index(AnyGuid, new NumberOfPackagesViewModel()) as ViewResult;

            Assert.IsType<NumberOfPackagesViewModel>(result.Model);
        }

        [Fact]
        public async Task PostCallsCorrectRequest()
        {
            await controller.Index(AnyGuid, new NumberOfPackagesViewModel
            {
                Number = 7
            });

            A.CallTo(
                () =>
                    client.SendAsync(A<string>.Ignored,
                        A<SetNumberOfPackagesByMovementId>.That.Matches(r => r.Id == AnyGuid
                                                                             && r.NumberOfPackages == 7)))
                                                                             .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task RedirectsToCorrectScreen()
        {
            var result = await controller.Index(AnyGuid, new NumberOfPackagesViewModel
            {
               Number = 5
            }) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "Index", "NumberOfCarriers");
        }
    }
}
