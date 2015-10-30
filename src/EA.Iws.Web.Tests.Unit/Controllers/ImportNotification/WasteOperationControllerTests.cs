namespace EA.Iws.Web.Tests.Unit.Controllers.ImportNotification
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Areas.ImportNotification.Controllers;
    using Areas.ImportNotification.ViewModels.WasteOperation;
    using Core.ImportNotification.Draft;
    using Core.OperationCodes;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Web.ViewModels.Shared;
    using Xunit;

    public class WasteOperationControllerTests
    {
        private readonly WasteOperationController controller;
        private readonly IMediator mediator;
        private readonly Guid importNotificationId = new Guid("A6386BA3-4070-4D83-99D1-54E9614A87EB");

        public WasteOperationControllerTests()
        {
            mediator = A.Fake<IMediator>();
            controller = new WasteOperationController(mediator);
        }

        [Fact]
        public async Task IndexPost_CallsMediatorWithWasteOperationData()
        {
            var model = new WasteOperationViewModel()
            {
                Codes = CheckBoxCollectionViewModel.CreateFromEnum<RecoveryCode>(),
                ImportNotificationId = importNotificationId,
                NotificationType = NotificationType.Recovery,
                TechnologyEmployed = "test",
                TechnologyEmployedUploadedLater = false
            };

            model.Codes.SetSelectedValues(new[] { RecoveryCode.R1, RecoveryCode.R2 });

            await controller.Index(importNotificationId, model);

            A.CallTo(() => mediator.SendAsync(A<SetDraftData<WasteOperation>>.That.Matches(p => 
                p.ImportNotificationId == importNotificationId &&
                p.Data.ImportNotificationId == importNotificationId &&
                p.Data.TechnologyEmployed == "test" &&
                p.Data.TechnologyEmployedUploadedLater == false &&
                p.Data.OperationCodes.Contains(1) &&
                p.Data.OperationCodes.Contains(2))))
                .MustHaveHappened();
        }
    }
}