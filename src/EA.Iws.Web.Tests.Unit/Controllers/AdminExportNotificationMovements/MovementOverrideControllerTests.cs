namespace EA.Iws.Web.Tests.Unit.Controllers.AdminExportNotificationMovements
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.AdminExportNotificationMovements.Controllers;
    using Areas.AdminExportNotificationMovements.ViewModels.MovementOverride;
    using Core.Movement;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Summary;
    using Requests.NotificationMovements.Capture;
    using Requests.NotificationMovements.Create;
    using Web.Infrastructure;
    using Xunit;

    public class MovementOverrideControllerTests
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;
        private readonly MovementOverrideController controller;
        private readonly Guid notificationId = Guid.NewGuid();
        private readonly Guid movementId = Guid.NewGuid();
        private readonly DateTime actualDate = new DateTime(2024, 1, 15);
        private readonly DateTime receivedDate = new DateTime(2024, 1, 20);
        private readonly DateTime recoveryDate = new DateTime(2024, 1, 25);

        public MovementOverrideControllerTests()
        {
            mediator = A.Fake<IMediator>();
            auditService = A.Fake<IAuditService>();
            controller = new MovementOverrideController(mediator, auditService);

            SetupDefaultMediatorResponses();
        }

        private void SetupDefaultMediatorResponses()
        {
            var movementData = new MovementReceiptAndRecoveryData
            {
                Id = movementId,
                NotificationId = notificationId,
                Number = 1,
                ActualDate = actualDate,
                PrenotificationDate = actualDate.AddDays(-3),
                NotificationType = NotificationType.Recovery,
                IsReceived = true,
                PossibleUnits = new List<ShipmentQuantityUnits> { ShipmentQuantityUnits.Tonnes, ShipmentQuantityUnits.Kilograms }
            };

            var summary = new InternalMovementSummary
            {
                NotificationNumber = "GB 0001 001234",
                TotalIntendedShipments = 100,
                TotalShipments = 10,
                ActiveLoadsPermitted = 5,
                QuantityRemaining = 500,
                QuantityReceived = 100,
                AverageTonnage = 10,
                DisplayUnit = ShipmentQuantityUnits.Tonnes,
                AverageDataUnit = ShipmentQuantityUnits.Tonnes
            };

            A.CallTo(() => mediator.SendAsync(A<GetMovementReceiptAndRecoveryData>.Ignored)).Returns(movementData);
            A.CallTo(() => mediator.SendAsync(A<GetInternalMovementSummary>.Ignored)).Returns(summary);
        }

        [Fact]
        public async Task Index_Get_ReturnsViewWithModel()
        {
            var result = await controller.Index(notificationId, movementId) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<IndexViewModel>(result.Model);
        }

        [Fact]
        public async Task Index_Get_CallsGetMovementReceiptAndRecoveryData()
        {
            await controller.Index(notificationId, movementId);

            A.CallTo(() => mediator.SendAsync(A<GetMovementReceiptAndRecoveryData>.That.Matches(r => r.MovementId == movementId)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Index_Post_WithAcceptedShipmentType_SetsIsReceivedTrue()
        {
            var model = CreateValidModel(ShipmentType.Accepted);

            await controller.Index(notificationId, movementId, model);

            A.CallTo(() => mediator.SendAsync(A<SetMovementReceiptAndRecoveryData>.That.Matches(
                r => r.Data.IsReceived == true && r.Data.IsRejected == false && r.Data.IsPartiallyRejected == false)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Index_Post_WithRejectedShipmentType_SetsIsRejectedTrue()
        {
            var model = CreateValidModel(ShipmentType.Rejected);
            model.RejectionReason = "Test rejection";
            model.RejectedQuantity = 10;
            model.RejectedUnits = ShipmentQuantityUnits.Tonnes;
            model.StatsMarking = "Illegal";

            await controller.Index(notificationId, movementId, model);

            A.CallTo(() => mediator.SendAsync(A<SetMovementReceiptAndRecoveryData>.That.Matches(
                r => r.Data.IsReceived == false && r.Data.IsRejected == true && r.Data.IsPartiallyRejected == false)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Index_Post_WithPartiallyRejectedShipmentType_SetsIsPartiallyRejectedTrue()
        {
            var model = CreateValidModel(ShipmentType.Partially);
            model.RejectionReason = "Test partial rejection";
            model.ActualQuantity = 5;
            model.Units = ShipmentQuantityUnits.Tonnes;
            model.RejectedQuantity = 5;
            model.RejectedUnits = ShipmentQuantityUnits.Tonnes;
            model.StatsMarking = "Illegal";

            await controller.Index(notificationId, movementId, model);

            A.CallTo(() => mediator.SendAsync(A<SetMovementReceiptAndRecoveryData>.That.Matches(
                r => r.Data.IsReceived == false && r.Data.IsRejected == false && r.Data.IsPartiallyRejected == true)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Index_Post_ValidModel_AddsEditedAudit()
        {
            var model = CreateValidModel(ShipmentType.Accepted);

            await controller.Index(notificationId, movementId, model);

            A.CallTo(() => auditService.AddMovementAudit(
                mediator,
                notificationId,
                model.ShipmentNumber,
                A<string>.Ignored,
                MovementAuditType.Edited))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Index_Post_ValidModel_RedirectsToCaptureMovementEdit()
        {
            var model = CreateValidModel(ShipmentType.Accepted);

            var result = await controller.Index(notificationId, movementId, model) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("Edit", result.RouteValues["action"]);
            Assert.Equal("CaptureMovement", result.RouteValues["controller"]);
            Assert.Equal(movementId, result.RouteValues["movementId"]);
        }

        [Fact]
        public async Task Index_Post_InvalidModelState_ReturnsView()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            controller.ModelState.AddModelError("Test", "Test error");

            var result = await controller.Index(notificationId, movementId, model) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<IndexViewModel>(result.Model);
        }

        [Fact]
        public async Task Index_Post_WithAcceptedShipmentType_ClearsRejectionFields()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.RejectionReason = null;
            model.RejectedQuantity = null;
            model.StatsMarking = null;

            await controller.Index(notificationId, movementId, model);

            A.CallTo(() => mediator.SendAsync(A<SetMovementReceiptAndRecoveryData>.That.Matches(
                r => r.Data.RejectionReason == null &&
                     r.Data.RejectedQuantity == null &&
                     r.Data.StatsMarking == null)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Index_Post_WithRejectedShipmentType_SetsRejectionDate()
        {
            var model = CreateValidModel(ShipmentType.Rejected);
            model.RejectionReason = "Test rejection";
            model.RejectedQuantity = 10;
            model.RejectedUnits = ShipmentQuantityUnits.Tonnes;
            model.StatsMarking = "Illegal";

            await controller.Index(notificationId, movementId, model);

            A.CallTo(() => mediator.SendAsync(A<SetMovementReceiptAndRecoveryData>.That.Matches(
                r => r.Data.RejectionDate == receivedDate && r.Data.ReceiptDate == null)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ChangeShipment_WithNewShipmentNumber_ExistingMovement_RedirectsToIndex()
        {
            A.CallTo(() => mediator.SendAsync(A<GetMovementIdIfExists>.Ignored)).Returns(movementId);

            var result = await controller.ChangeShipment(notificationId, null, 2) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
        }

        [Fact]
        public async Task ChangeShipment_WithNewShipmentNumber_NonExistingMovement_RedirectsToCreate()
        {
            A.CallTo(() => mediator.SendAsync(A<GetMovementIdIfExists>.Ignored)).Returns((Guid?)null);

            var result = await controller.ChangeShipment(notificationId, null, 2) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("Create", result.RouteValues["action"]);
            Assert.Equal("CaptureMovement", result.RouteValues["controller"]);
        }

        private IndexViewModel CreateValidModel(ShipmentType shipmentType)
        {
            return new IndexViewModel
            {
                ShipmentNumber = 1,
                NotificationType = NotificationType.Recovery,
                ActualShipmentDate = actualDate,
                PrenotificationDate = actualDate.AddDays(-3),
                ReceivedDate = receivedDate,
                ShipmentTypes = shipmentType,
                ActualQuantity = 10,
                Units = ShipmentQuantityUnits.Tonnes,
                Date = recoveryDate,
                PossibleUnits = new List<ShipmentQuantityUnits> { ShipmentQuantityUnits.Tonnes }
            };
        }
    }
}