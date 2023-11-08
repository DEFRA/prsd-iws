namespace EA.Iws.Web.Tests.Unit.Controllers.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.ImportNotification.Controllers;
    using Core.ImportNotification.Draft;
    using EA.Iws.Core.WasteComponentType;
    using EA.Iws.Web.Areas.ImportNotification.ViewModels.WasteComponent;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Web.ViewModels.Shared;
    using Xunit;

    public class WasteComponentControllerTests
    {
        private readonly WasteComponentController controller;
        private readonly IMediator mediator;
        private readonly Guid importNotificationId = new Guid("A6386BA3-4070-4D83-99D1-54E9614A87EB");

        public WasteComponentControllerTests()
        {
            mediator = A.Fake<IMediator>();
            controller = new WasteComponentController(mediator);
        }

        [Fact]
        public async Task Index_ReturnsView()
        {
            var result = await controller.Index(importNotificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task IndexPost_CallsMediatorWithWasteOperationData()
        {
            var model = new WasteComponentViewModel()
            {
                Codes = new List<KeyValuePairViewModel<WasteComponentType, bool>>
                {
                    new KeyValuePairViewModel<WasteComponentType, bool>(WasteComponentType.NORM, true),
                    new KeyValuePairViewModel<WasteComponentType, bool>(WasteComponentType.FGasODS, false),
                    new KeyValuePairViewModel<WasteComponentType, bool>(WasteComponentType.Mercury, false)
                },
                ImportNotificationId = importNotificationId
            };

            await controller.Index(importNotificationId, model);

            A.CallTo(() => mediator.SendAsync(A<SetDraftData<WasteComponent>>.That.Matches(p =>
                                                                                            p.ImportNotificationId == importNotificationId &&
                                                                                            p.Data.ImportNotificationId == importNotificationId &&
                                                                                            p.Data.WasteComponentTypes.Contains(WasteComponentType.NORM))))
                                                                                   .MustHaveHappened();
        }
    }
}
