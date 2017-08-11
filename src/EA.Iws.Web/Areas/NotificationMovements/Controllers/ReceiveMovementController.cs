namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using Core.Shared;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement.Receive;
    using Requests.NotificationMovements;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.ReceiveMovement;

    [AuthorizeActivity(typeof(GetSubmittedMovementsByNotificationId))]
    public class ReceiveMovementController : Controller
    {
        private readonly IMediator mediator;

        public ReceiveMovementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var receivedResult = await mediator.SendAsync(new GetSubmittedMovementsByNotificationId(notificationId));

            var recoveryResult = await mediator.SendAsync(new GetReceivedMovements(notificationId));

            return View(new MovementReceiptViewModel(notificationId, receivedResult, recoveryResult));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid notificationId, MovementReceiptViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.ReceiveShipments.Any(s => s.IsSelected))
            {
                return RedirectToAction("CertificateTypes", "ReceiptRecovery", model.ReceiveShipments.Where(s => s.IsSelected).Select(s => s.Id).ToRouteValueDictionary("movementIds"));
            }

            if (model.RecoveryShipments.Any(s => s.IsSelected))
            {
                return RedirectToAction("Index", "ReceiptRecovery", model.RecoveryShipments.Where(s => s.IsSelected).Select(s => s.Id).ToRouteValueDictionary("movementIds"));
            }
            return View(model);
        }      
    }
}