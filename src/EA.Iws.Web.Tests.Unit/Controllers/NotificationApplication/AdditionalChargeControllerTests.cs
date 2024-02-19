namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.Requests.Notification;
    using EA.Iws.Web.Areas.AdminExportAssessment.Controllers;
    using EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.AdditionalCharge;
    using EA.Prsd.Core.Mediator;
    using FakeItEasy;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class AdditionalChargeControllerTests
    {
        private readonly IMediator mediator;
        private readonly AdditionalChargeController additionalChargeController;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");

        public AdditionalChargeControllerTests()
        {
            mediator = A.Fake<IMediator>();

            A.CallTo(() => mediator.SendAsync(A<GetNotificationAdditionalCharges>.Ignored))
            .Returns(new List<NotificationAdditionalChargeForDisplay>
            {
                    new NotificationAdditionalChargeForDisplay()
                    {
                        ChangeDetailType = AdditionalChargeType.EditExportDetails,
                        ChargeAmount = 100,
                        ChargeDate = DateTime.Now,
                        Comments = "Test"
                    }
                });

            additionalChargeController = new AdditionalChargeController(mediator);
        }

        [Fact]
        public async Task ValidModel_ReturnsView()
        {
            var result = await additionalChargeController.Index(notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task GetIndex_ReturnsViewModel()
        {
            var result = await additionalChargeController.Index(notificationId) as ViewResult;

            Assert.IsType<AdditionalChargeViewModel>(result.Model);

            var model = result.Model as AdditionalChargeViewModel;

            Assert.True(model.AdditionalChargeData.Count > 0);
        }
    }
}
