namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.NotificationApplication.Controllers;
    using Core.CustomsOffice;
    using FakeItEasy;
    using Requests.CustomsOffice;
    using TestHelpers;
    using Xunit;

    public class CustomsOfficeControllerTests
    {
        private readonly IIwsClient client;
        private readonly CustomsOfficeController controller;
        private readonly Guid guid = Guid.Empty;
        private const string IntendedShipmentsAction = "Index";
        private const string IntendedShipmentsController = "Shipment";

        public CustomsOfficeControllerTests()
        {
            client = A.Fake<IIwsClient>();
            controller = new CustomsOfficeController(() => client);
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
                () => client.SendAsync(A<string>.Ignored, A<GetCustomsCompletionStatusByNotificationId>.Ignored)).Returns(new CustomsOfficeCompletionStatus
                {
                    CustomsOfficesCompleted = CustomsOffices.None,
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
                () => client.SendAsync(A<string>.Ignored, A<GetCustomsCompletionStatusByNotificationId>.Ignored)).Returns(new CustomsOfficeCompletionStatus
                {
                    CustomsOfficesCompleted = CustomsOffices.None,
                    CustomsOfficesRequired = CustomsOffices.Entry
                });

            var result = await controller.Index(guid) as RedirectToRouteResult;

            result.AssertControllerReturn("Index", "EntryCustomsOffice");
        }

        [Fact]
        public async Task Index_CustomOfficesExit_RedirectsToExitCustomsOfficePage()
        {
            A.CallTo(
                () => client.SendAsync(A<string>.Ignored, A<GetCustomsCompletionStatusByNotificationId>.Ignored)).Returns(new CustomsOfficeCompletionStatus
                {
                    CustomsOfficesCompleted = CustomsOffices.None,
                    CustomsOfficesRequired = CustomsOffices.Exit
                });

            var result = await controller.Index(guid) as RedirectToRouteResult;

            result.AssertControllerReturn("Index", "ExitCustomsOffice");
        }

        [Fact]
        public async Task Index_CustomOfficesEntryAndExit_RedirectsToExitCustomsOfficePage()
        {
            A.CallTo(
                () => client.SendAsync(A<string>.Ignored, A<GetCustomsCompletionStatusByNotificationId>.Ignored)).Returns(new CustomsOfficeCompletionStatus
                {
                    CustomsOfficesCompleted = CustomsOffices.None,
                    CustomsOfficesRequired = CustomsOffices.EntryAndExit
                });

            var result = await controller.Index(guid) as RedirectToRouteResult;

            result.AssertControllerReturn("Index", "ExitCustomsOffice");
        }

        [Fact]
        public async Task Index_CustomOfficesExitAlreadyCompleted_RedirectsToExit()
        {
            A.CallTo(
               () => client.SendAsync(A<string>.Ignored, A<GetCustomsCompletionStatusByNotificationId>.Ignored)).Returns(new CustomsOfficeCompletionStatus
               {
                   CustomsOfficesCompleted = CustomsOffices.Exit,
                   CustomsOfficesRequired = CustomsOffices.Exit
               });

            var result = await controller.Index(guid) as RedirectToRouteResult;

            result.AssertControllerReturn("Index", "ExitCustomsOffice");
            Assert.Equal(guid, result.RouteValues["id"]);
        }

        [Fact]
        public async Task Index_CustomOfficesEntryAndExitWithExitAlreadyCompleted_RedirectsToExit()
        {
            A.CallTo(
               () => client.SendAsync(A<string>.Ignored, A<GetCustomsCompletionStatusByNotificationId>.Ignored)).Returns(new CustomsOfficeCompletionStatus
               {
                   CustomsOfficesCompleted = CustomsOffices.Exit,
                   CustomsOfficesRequired = CustomsOffices.EntryAndExit
               });

            var result = await controller.Index(guid) as RedirectToRouteResult;

            result.AssertControllerReturn("Index", "ExitCustomsOffice");
            Assert.Equal(guid, result.RouteValues["id"]);
        }
    }
}
