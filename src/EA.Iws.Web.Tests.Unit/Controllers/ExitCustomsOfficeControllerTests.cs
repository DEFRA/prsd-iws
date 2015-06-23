namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels;
    using Areas.NotificationApplication.ViewModels.CustomsOffice;
    using Core;
    using Core.CustomsOffice;
    using Core.Shared;
    using FakeItEasy;
    using Requests.CustomsOffice;
    using TestHelpers;
    using Xunit;

    public class ExitCustomsOfficeControllerTests
    {
        private readonly Guid exitCustomsOfficeRequiredGuid = new Guid("2FEA1048-C1F8-490E-BA7D-58F31053F84D");
        private readonly Guid exitCustomsOfficeNotRequiredGuid = new Guid("3ADD18F4-BDDF-4A98-A6EB-81631F5DF2EC");
        private readonly IIwsClient client;
        private readonly ExitCustomsOfficeController controller;

        public ExitCustomsOfficeControllerTests()
        {
            client = A.Fake<IIwsClient>();
            controller = new ExitCustomsOfficeController(() => client);
        }
            
        [Fact]
        public async Task Add_ExitCustomsOfficeRequired_ReturnsView()
        {
            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<GetExitCustomsOfficeAddDataByNotificationId>.Ignored))
                .Returns(new ExitCustomsOfficeAddData
                {
                    CustomsOffices = CustomsOffices.Exit,
                    Countries = new[] { new CountryData { Id = Guid.Empty, Name = "Test" } }
                });

            var result = await controller.Add(exitCustomsOfficeRequiredGuid) as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Add_NoCustomsOfficeRequired_RedirectsToIndex()
        {
            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<GetExitCustomsOfficeAddDataByNotificationId>.Ignored))
                .Returns(new ExitCustomsOfficeAddData
                {
                    CustomsOffices = CustomsOffices.None
                });

            var result = await controller.Add(exitCustomsOfficeNotRequiredGuid) as RedirectToRouteResult;

            result.AssertControllerReturn("Index", "CustomsOffice");
        }

        [Fact]
        public async Task Add_ExitCustomsOfficeRequiredNoCustomsOfficeSet_ReturnsViewWithModel()
        {
            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<GetExitCustomsOfficeAddDataByNotificationId>.Ignored))
                .Returns(new ExitCustomsOfficeAddData
                {
                    CustomsOffices = CustomsOffices.Exit,
                    Countries = new[] { new CountryData { Id = Guid.Empty, Name = "Test" } }
                });

            var result = await controller.Add(exitCustomsOfficeRequiredGuid) as ViewResult;

            Assert.IsType<CustomsOfficeViewModel>(result.Model);

            var model = result.Model as CustomsOfficeViewModel;
            Assert.Equal(1, model.Countries.Count());
        }
    }
}
