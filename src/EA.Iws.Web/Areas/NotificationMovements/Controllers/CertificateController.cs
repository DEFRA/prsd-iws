namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using Core.Shared;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement.Receive;
    using Requests.Notification;
    using Requests.NotificationMovements;
    using System;
    using System.Linq;
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
        public ActionResult CertificateTypes(Guid notificationId)
        {
            CertificationSelectionViewModel model = new CertificationSelectionViewModel();
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
            return RedirectToAction("Shipments", new { notificationId = notificationId, certificate = model.Certificate });
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
        public ActionResult Shipments(Guid notificationId, ShipmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            TempData[CertificateKey] = model.Certificate;

            if (model.Certificate == CertificateType.ReceiptRecovery && model.ReceiveShipments.Any(s => s.IsSelected))
            {
                return RedirectToAction("Index", "ReceiptRecovery", model.ReceiveShipments.Where(s => s.IsSelected).Select(s => s.Id).ToRouteValueDictionary("movementIds"));
            }
            else if (model.Certificate == CertificateType.Receipt && model.ReceiveShipments.Any(s => s.IsSelected))
            {
                return RedirectToAction("Receipt", "ReceiptRecovery", model.ReceiveShipments.Where(s => s.IsSelected).Select(s => s.Id).ToRouteValueDictionary("movementIds"));
            }
            else if (model.Certificate == CertificateType.Recovery && model.RecoveryShipments.Any(s => s.IsSelected))
            {
                return RedirectToAction("Recovery", "ReceiptRecovery", model.RecoveryShipments.Where(s => s.IsSelected).Select(s => s.Id).ToRouteValueDictionary("movementIds"));
            }
            return View(model);
        }
    }
}