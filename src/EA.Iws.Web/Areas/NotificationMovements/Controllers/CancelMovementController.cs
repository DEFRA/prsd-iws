namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels.CancelMovement;

    [Authorize]
    public class CancelMovementController : Controller
    {
        private readonly IMediator mediator;
        private const string SubmittedMovementList = "SubmittedMovementList";

        public CancelMovementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var result = await mediator.SendAsync(new GetSubmittedMovements(notificationId));

            var model = new CancelMovementsViewModel(notificationId, result);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(CancelMovementsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await Index(model.NotificationId);
            }

            var selectedMovements = model.SubmittedMovements.Where(m => m.IsSelected).Select(p => new CancelMovementsList { MovementId = p.MovementId, Number = p.Number }).ToList();
            TempData.Add(SubmittedMovementList, selectedMovements);
            return RedirectToAction("Confirm", "CancelMovement", new { notificationId = model.NotificationId });
        }

        [HttpGet]
        public ActionResult Confirm(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(SubmittedMovementList, out result))
            {
                var model = new ConfirmCancelMovementsViewModel(notificationId, result as List<CancelMovementsList>);
                return View(model);
            }
            return RedirectToAction("Index", "CancelMovement", new { notificationId });
        }
    }
}