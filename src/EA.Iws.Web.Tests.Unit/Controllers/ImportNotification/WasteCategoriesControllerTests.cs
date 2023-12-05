namespace EA.Iws.Web.Tests.Unit.Controllers.ImportNotification
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.ImportNotification.Controllers;
    using Core.ImportNotification.Draft;
    using EA.Iws.Core.WasteType;
    using EA.Iws.Web.Areas.ImportNotification.ViewModels.WasteCategories;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Xunit;

    public class WasteCategoriesControllerTests
    {
        private readonly WasteCategoriesController controller;
        private readonly IMediator mediator;
        private readonly Guid importNotificationId = new Guid("A6386BA3-4070-4D83-99D1-54E9614A87EB");

        public WasteCategoriesControllerTests()
        {
            mediator = A.Fake<IMediator>();
            controller = new WasteCategoriesController(mediator);
        }

        [Fact]
        public async Task Index_ReturnsView()
        {
            var result = await controller.Index(importNotificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task IndexPost_CallsMediatorWithWasteCategoriesData()
        {
            var model = new WasteCategoriesViewModel();
            model.WasteCategories = new Web.ViewModels.Shared.RadioButtonStringCollectionViewModel() { SelectedValue = WasteCategoryType.Oils.ToString() };

            await controller.Index(importNotificationId, model);

            A.CallTo(() => mediator.SendAsync(A<SetDraftData<WasteCategories>>
                                   .That
                                   .Matches(p => p.ImportNotificationId == importNotificationId &&
                                                 p.Data.ImportNotificationId == importNotificationId &&
                                                 p.Data.WasteCategoryType.ToString().Contains(WasteCategoryType.Oils.ToString()))))
                                   .MustHaveHappened();
        }
    }
}
