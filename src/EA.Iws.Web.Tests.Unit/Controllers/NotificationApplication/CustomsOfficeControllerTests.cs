namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.NotificationApplication.Controllers;
    using Core.CustomsOffice;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;
    using TestHelpers;
    using Xunit;

    public class CustomsOfficeControllerTests
    {
        private readonly IMediator mediator;
        private readonly CustomsOfficeController controller;
        private readonly Guid guid = Guid.Empty;
        private const string IntendedShipmentsAction = "Index";
        private const string IntendedShipmentsController = "Shipment";

        public CustomsOfficeControllerTests()
        {
            mediator = A.Fake<IMediator>();
            controller = new CustomsOfficeController(mediator);
        }

        [Fact]
        public async Task Index_NoTransportRouteSet_RedirectsToTransportRouteSummary()
        {
            var result = await controller.Index(guid) as RedirectToRouteResult;

            result.AssertControllerReturn("Summary", "TransportRoute");
            Assert.Equal(guid, result.RouteValues["id"]);
        }

        [Fact]
        public async Task Index_NoCustomsOfficeRequired_RedirectsToIntendedShipments()
        {
            A.CallTo(
                () => mediator.SendAsync(A<GetCustomsCompletionStatusByNotificationId>.Ignored)).Returns(new CustomsOfficeCompletionStatus
                {
                    CustomsOfficesRequired = CustomsOffices.None
                });

            var result = await controller.Index(guid) as RedirectToRouteResult;

            result.AssertControllerReturn("NoCustomsOffice", "CustomsOffice");
            Assert.Equal(guid, result.RouteValues["id"]);
        }

        [Fact]
        public async Task Index_CustomOfficesEntry_RedirectsToEntryCustomsOfficePage()
        {
            A.CallTo(
                () => mediator.SendAsync(A<GetCustomsCompletionStatusByNotificationId>.Ignored)).Returns(new CustomsOfficeCompletionStatus
                {
                    CustomsOfficesRequired = CustomsOffices.Entry
                });

            var result = await controller.Index(guid) as RedirectToRouteResult;

            result.AssertControllerReturn("Index", "EntryCustomsOffice");
        }

        [Fact]
        public async Task Index_CustomOfficesExit_RedirectsToExitCustomsOfficePage()
        {
            A.CallTo(
                () => mediator.SendAsync(A<GetCustomsCompletionStatusByNotificationId>.Ignored)).Returns(new CustomsOfficeCompletionStatus
                {
                    CustomsOfficesRequired = CustomsOffices.Exit
                });

            var result = await controller.Index(guid) as RedirectToRouteResult;

            result.AssertControllerReturn("Index", "ExitCustomsOffice");
        }

        [Fact]
        public async Task Index_CustomOfficesEntryAndExit_RedirectsToEntryCustomsOfficePage()
        {
            A.CallTo(
                () => mediator.SendAsync(A<GetCustomsCompletionStatusByNotificationId>.Ignored)).Returns(new CustomsOfficeCompletionStatus
                {
                    CustomsOfficesRequired = CustomsOffices.EntryAndExit
                });

            var result = await controller.Index(guid) as RedirectToRouteResult;

            result.AssertControllerReturn("Index", "EntryCustomsOffice");
        }

        [Fact]
        public async Task Index_CustomOfficesExitAlreadyCompleted_RedirectsToExit()
        {
            A.CallTo(
               () => mediator.SendAsync(A<GetCustomsCompletionStatusByNotificationId>.Ignored)).Returns(new CustomsOfficeCompletionStatus
               {
                   CustomsOfficesRequired = CustomsOffices.Exit
               });

            var result = await controller.Index(guid) as RedirectToRouteResult;

            result.AssertControllerReturn("Index", "ExitCustomsOffice");
            Assert.Equal(guid, result.RouteValues["id"]);
        }

        [Fact]
        public async Task Index_CustomOfficesEntryAndExitWithExitAlreadyCompleted_RedirectsToEntry()
        {
            A.CallTo(
               () => mediator.SendAsync(A<GetCustomsCompletionStatusByNotificationId>.Ignored)).Returns(new CustomsOfficeCompletionStatus
               {
                   CustomsOfficesRequired = CustomsOffices.EntryAndExit
               });

            var result = await controller.Index(guid) as RedirectToRouteResult;

            result.AssertControllerReturn("Index", "EntryCustomsOffice");
            Assert.Equal(guid, result.RouteValues["id"]);
        }
    }
}
