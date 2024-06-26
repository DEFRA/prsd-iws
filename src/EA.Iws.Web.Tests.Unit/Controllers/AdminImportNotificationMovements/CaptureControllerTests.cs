﻿namespace EA.Iws.Web.Tests.Unit.Controllers.AdminImportNotificationMovements
{
    using System;
    using System.Threading.Tasks;
    using Areas.AdminImportNotificationMovements.Controllers;
    using Areas.AdminImportNotificationMovements.ViewModels.Capture;
    using Core.Movement;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Capture;
    using Web.Infrastructure;
    using Web.Infrastructure.Authorization;
    using Xunit;
    public class CaptureControllerTests
    {
        private readonly IMediator mediator;
        private readonly CaptureController controller;
        private readonly Guid notificationId = new Guid("AF81049E-4F94-479A-921F-27B27C4F8BB9");
        private readonly IAuditService auditService;
        private readonly AuthorizationService authorizationService;
        private readonly DateTime receivedDate = new DateTime(2019, 3, 2);
        private readonly DateTime recoveredDate = new DateTime(2019, 3, 2);
        private readonly DateTime prenotifiedDate = new DateTime(2019, 3, 2);
        private readonly DateTime actualDate = new DateTime(2019, 3, 2);
        private readonly DateTime rejectedDate = new DateTime(2019, 3, 2);
        private Guid? movementId = null;

        public CaptureControllerTests()
        {
            mediator = A.Fake<IMediator>();
            this.auditService = A.Fake<IAuditService>();
            this.authorizationService = A.Fake<AuthorizationService>();
            controller = new CaptureController(mediator, authorizationService, auditService);
        }

        [Fact]
        public async Task Add_PrenotifiedAudit()
        {
            var model = new CaptureViewModel
            {
                NotificationId = notificationId,
                ShipmentNumber = 1,
                HasNoPrenotification = false,
                ActualShipmentDate = new Web.ViewModels.Shared.MaskedDateInputViewModel(actualDate)
            };

            A.CallTo(() => mediator.SendAsync(A<GetMovementIdIfExists>.Ignored)).Returns(movementId);

            var result = await controller.Create(notificationId, model);

            A.CallTo(() => this.auditService.AddImportMovementAudit(mediator, notificationId, 1, controller.User.GetUserId(), MovementAuditType.Prenotified)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Add_NoPrenotificationReceivedAudit()
        {
            var model = new CaptureViewModel
            {
                NotificationId = notificationId,
                ShipmentNumber = 1,
                HasNoPrenotification = true,
                ActualShipmentDate = new Web.ViewModels.Shared.MaskedDateInputViewModel(actualDate)
            };

            A.CallTo(() => mediator.SendAsync(A<GetMovementIdIfExists>.Ignored)).Returns(movementId);

            var result = await controller.Create(notificationId, model);

            A.CallTo(() => this.auditService.AddImportMovementAudit(mediator, notificationId, 1, controller.User.GetUserId(), MovementAuditType.NoPrenotificationReceived)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Add_RejectedAudit()
        {
            var model = new CaptureViewModel
            {
                NotificationId = notificationId,
                ShipmentNumber = 1,
                HasNoPrenotification = true,
                ActualShipmentDate = new Web.ViewModels.Shared.MaskedDateInputViewModel(actualDate),
                Receipt = new ReceiptViewModel
                {
                    WasAccepted = false,
                    ReceivedDate = new Web.ViewModels.Shared.MaskedDateInputViewModel(rejectedDate),
                    RejectionReason = "TestRejection",
                    RejectedQuantity = 1,
                    RejectedUnits = ShipmentQuantityUnits.Tonnes,
                    ShipmentTypes = ShipmentType.Rejected
                }
            };

            A.CallTo(() => mediator.SendAsync(A<GetMovementIdIfExists>.Ignored)).Returns(movementId);

            var result = await controller.Create(notificationId, model);

            A.CallTo(() => this.auditService.AddImportMovementAudit(mediator, notificationId, 1, controller.User.GetUserId(), MovementAuditType.Rejected)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Add_PartialRejectedAudit()
        {
            var model = new CaptureViewModel
            {
                NotificationId = notificationId,
                ShipmentNumber = 1,
                HasNoPrenotification = true,
                ActualShipmentDate = new Web.ViewModels.Shared.MaskedDateInputViewModel(actualDate),
                Receipt = new ReceiptViewModel
                {
                    WasAccepted = false,
                    ReceivedDate = new Web.ViewModels.Shared.MaskedDateInputViewModel(rejectedDate),
                    RejectionReason = "TestRejection",
                    ActualQuantity = 10,
                    ActualUnits = ShipmentQuantityUnits.Tonnes,
                    RejectedQuantity = 5,
                    RejectedUnits = ShipmentQuantityUnits.Tonnes,
                    ShipmentTypes = ShipmentType.Partially
                },
                Recovery = new RecoveryViewModel
                {
                    RecoveryDate = new Web.ViewModels.Shared.MaskedDateInputViewModel(recoveredDate),
                    NotificationType = NotificationType.Disposal,
                    IsShipmentFullRejected = false
                }
            };

            A.CallTo(() => mediator.SendAsync(A<GetMovementIdIfExists>.Ignored)).Returns(movementId);

            var result = await controller.Create(notificationId, model);

            A.CallTo(() => this.auditService.AddImportMovementAudit(mediator, notificationId, 1, controller.User.GetUserId(), MovementAuditType.PartiallyRejected)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Add_ReceivedAudit()
        {
            var model = new CaptureViewModel
            {
                NotificationId = notificationId,
                ShipmentNumber = 1,
                HasNoPrenotification = true,
                ActualShipmentDate = new Web.ViewModels.Shared.MaskedDateInputViewModel(actualDate),
                Receipt = new ReceiptViewModel
                {
                    WasAccepted = true,
                    ReceivedDate = new Web.ViewModels.Shared.MaskedDateInputViewModel(receivedDate),
                    ActualQuantity = 1,
                    ActualUnits = ShipmentQuantityUnits.Kilograms
                }
            };

            A.CallTo(() => mediator.SendAsync(A<GetMovementIdIfExists>.Ignored)).Returns(movementId);

            var result = await controller.Create(notificationId, model);

            A.CallTo(() => this.auditService.AddImportMovementAudit(mediator, notificationId, 1, controller.User.GetUserId(), MovementAuditType.Received)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Add_RecoveredAudit()
        {
            var model = new CaptureViewModel
            {
                NotificationId = notificationId,
                ShipmentNumber = 1,
                HasNoPrenotification = true,
                ActualShipmentDate = new Web.ViewModels.Shared.MaskedDateInputViewModel(actualDate),
                Receipt = new ReceiptViewModel
                {
                    WasAccepted = true,
                    ReceivedDate = new Web.ViewModels.Shared.MaskedDateInputViewModel(receivedDate),
                    ActualQuantity = 1,
                    ActualUnits = ShipmentQuantityUnits.Kilograms
                },
                Recovery = new RecoveryViewModel
                {
                    RecoveryDate = new Web.ViewModels.Shared.MaskedDateInputViewModel(recoveredDate)
                },
                NotificationType = NotificationType.Recovery
            };

            A.CallTo(() => mediator.SendAsync(A<GetMovementIdIfExists>.Ignored)).Returns(movementId);

            var result = await controller.Create(notificationId, model);

            A.CallTo(() => this.auditService.AddImportMovementAudit(mediator, notificationId, 1, controller.User.GetUserId(), MovementAuditType.Recovered)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Add_DisposedAudit()
        {
            var model = new CaptureViewModel
            {
                NotificationId = notificationId,
                ShipmentNumber = 1,
                HasNoPrenotification = true,
                ActualShipmentDate = new Web.ViewModels.Shared.MaskedDateInputViewModel(actualDate),
                Receipt = new ReceiptViewModel
                {
                    WasAccepted = true,
                    ReceivedDate = new Web.ViewModels.Shared.MaskedDateInputViewModel(receivedDate),
                    ActualQuantity = 1,
                    ActualUnits = ShipmentQuantityUnits.Kilograms
                },
                Recovery = new RecoveryViewModel
                {
                    RecoveryDate = new Web.ViewModels.Shared.MaskedDateInputViewModel(recoveredDate)
                },
                NotificationType = NotificationType.Disposal
            };

            A.CallTo(() => mediator.SendAsync(A<GetMovementIdIfExists>.Ignored)).Returns(movementId);

            var result = await controller.Create(notificationId, model);

            A.CallTo(() => this.auditService.AddImportMovementAudit(mediator, notificationId, 1, controller.User.GetUserId(), MovementAuditType.Disposed)).MustHaveHappenedOnceExactly();
        }
    }
}
