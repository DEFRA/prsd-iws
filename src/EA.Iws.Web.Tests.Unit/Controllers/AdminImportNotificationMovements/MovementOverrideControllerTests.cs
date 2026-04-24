namespace EA.Iws.Web.Tests.Unit.Controllers.AdminImportNotificationMovements
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.AdminImportNotificationMovements.Controllers;
    using Areas.AdminImportNotificationMovements.ViewModels.MovementOverride;
    using Core.ImportMovement;
    using Core.ImportNotificationMovements;
    using Core.Movement;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement;
    using Requests.ImportMovement.Capture;
    using Requests.ImportNotificationMovements;
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
            var movementData = new ImportMovementSummaryData
            {
                MovementId = movementId,
                Data = new ImportMovementData
                {
                    NotificationId = notificationId,
                    Number = 1,
                    ActualDate = actualDate,
                    PreNotificationDate = actualDate.AddDays(-3),
                    NotificationType = NotificationType.Recovery
                },
                ReceiptData = new ImportMovementReceiptData
                {
                    IsReceived = true,
                    PossibleUnits = new List<ShipmentQuantityUnits> { ShipmentQuantityUnits.Tonnes, ShipmentQuantityUnits.Kilograms }
                },
                RecoveryData = new ImportMovementRecoveryData()
            };

            var summary = new Summary
            {
                NotificationNumber = "GB 0001 001234",
                IntendedShipments = 100,
                UsedShipments = 10,
                QuantityRemainingTotal = 500,
                QuantityReceivedTotal = 100,
                AverageTonnage = 10,
                DisplayUnit = ShipmentQuantityUnits.Tonnes,
                AverageDataUnit = ShipmentQuantityUnits.Tonnes
            };

            A.CallTo(() => mediator.SendAsync(A<GetImportMovementReceiptAndRecoveryData>.Ignored)).Returns(movementData);
            A.CallTo(() => mediator.SendAsync(A<GetImportMovementsSummary>.Ignored)).Returns(summary);
        }

        [Fact]
        public async Task Index_Get_ReturnsViewWithModel()
        {
            var result = await controller.Index(notificationId, movementId) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<IndexViewModel>(result.Model);
        }

        [Fact]
        public async Task Index_Get_CallsGetImportMovementReceiptAndRecoveryData()
        {
            await controller.Index(notificationId, movementId);

            A.CallTo(() => mediator.SendAsync(A<GetImportMovementReceiptAndRecoveryData>.That.Matches(r => r.ImportMovementId == movementId)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Index_Post_WithAcceptedShipmentType_SetsIsReceivedTrue()
        {
            var model = CreateValidModel(ShipmentType.Accepted);

            await controller.Index(notificationId, movementId, model);

            A.CallTo(() => mediator.SendAsync(A<SetImportMovementReceiptAndRecoveryData>.That.Matches(
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

            A.CallTo(() => mediator.SendAsync(A<SetImportMovementReceiptAndRecoveryData>.That.Matches(
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

            A.CallTo(() => mediator.SendAsync(A<SetImportMovementReceiptAndRecoveryData>.That.Matches(
                r => r.Data.IsReceived == false && r.Data.IsRejected == false && r.Data.IsPartiallyRejected == true)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Index_Post_ValidModel_AddsEditedAudit()
        {
            var model = CreateValidModel(ShipmentType.Accepted);

            await controller.Index(notificationId, movementId, model);

            A.CallTo(() => auditService.AddImportMovementAudit(
                mediator,
                notificationId,
                model.ShipmentNumber,
                A<string>.Ignored,
                MovementAuditType.Edited))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Index_Post_ValidModel_RedirectsToCaptureEdit()
        {
            var model = CreateValidModel(ShipmentType.Accepted);

            var result = await controller.Index(notificationId, movementId, model) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("Edit", result.RouteValues["action"]);
            Assert.Equal("Capture", result.RouteValues["controller"]);
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
        public async Task ChangeShipment_WithNewShipmentNumber_ExistingMovement_RedirectsToIndex()
        {
            A.CallTo(() => mediator.SendAsync(A<GetImportMovementIdIfExists>.Ignored)).Returns(movementId);

            var result = await controller.ChangeShipment(notificationId, null, 2) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
        }

        [Fact]
        public async Task ChangeShipment_WithNewShipmentNumber_NonExistingMovement_RedirectsToCreate()
        {
            A.CallTo(() => mediator.SendAsync(A<GetImportMovementIdIfExists>.Ignored)).Returns((Guid?)null);

            var result = await controller.ChangeShipment(notificationId, null, 2) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("Create", result.RouteValues["action"]);
            Assert.Equal("Capture", result.RouteValues["controller"]);
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