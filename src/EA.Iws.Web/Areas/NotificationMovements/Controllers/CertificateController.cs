namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using Core.Shared;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Receive;
    using Requests.Notification;
    using Requests.NotificationMovements;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Certificate;

    [AuthorizeActivity(typeof(GetSubmittedMovementsByNotificationId))]
    public class CertificateController : Controller
    {
        private readonly IMediator mediator;
        private const string CertificateKey = "CertificateType";
        public CertificateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> CertificateTypes(Guid notificationId)
        {
            var model = new CertificationSelectionViewModel
            {
                NotificationId = notificationId,
                NotificationType = await mediator.SendAsync(new GetNotificationType(notificationId))
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CertificateTypes(Guid notificationId, CertificationSelectionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return RedirectToAction("Shipments", new { notificationId, certificate = model.Certificate });
        }

        [HttpGet]
        public async Task<ActionResult> Shipments(Guid notificationId, CertificateType certificate)
        {
            var notificationType = await mediator.SendAsync(new GetNotificationType(notificationId));

            if (certificate == CertificateType.Receipt || certificate == CertificateType.ReceiptRecovery)
            {
                var receivedResult = await mediator.SendAsync(new GetSubmittedMovementsByNotificationId(notificationId));
                return View(new ShipmentViewModel(notificationId, notificationType, certificate, receivedResult));
            }

            if (certificate == CertificateType.Recovery)
            {
                var recoveryResult = await mediator.SendAsync(new GetReceivedMovements(notificationId));
                return View(new ShipmentViewModel(notificationId, notificationType, certificate, recoveryResult));
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Shipments(Guid notificationId, ShipmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            Guid movementId;

            if (model.Certificate == CertificateType.Recovery)
            {
                movementId = model.RecoveryShipments.SelectedValue;
            }
            else
            {
                movementId = model.ReceiveShipments.SelectedValue;
            }

            TempData[CertificateKey] = model.Certificate;

            if (model.Certificate == CertificateType.ReceiptRecovery)
            {
                return RedirectToAction("Index", "ReceiptRecovery", new { movementId = model.ReceiveShipments.SelectedValue});
            }

            if (model.Certificate == CertificateType.Receipt)
            {
                return RedirectToAction("Receipt", "ReceiptRecovery", new { movementId = model.ReceiveShipments.SelectedValue});
            }

            if (model.Certificate == CertificateType.Recovery)
            {
                return RedirectToAction("Recovery", "ReceiptRecovery", new { movementId = model.RecoveryShipments.SelectedValue});
            }

            return View(model);
        }
    }
}