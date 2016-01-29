namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationMovements;
    using ViewModels.Shipments;

    [Authorize(Roles = "internal")]
    public class ShipmentsController : Controller
    {
        private readonly IMediator mediator;

        public ShipmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, int? status)
        {
            var data = await mediator.SendAsync(new GetImportMovementsSummaryTable(id, (ImportMovementStatus?)status));

            var model = new ShipmentsTableViewModel(data);
            model.SelectedMovementStatus = (ImportMovementStatus?)status;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public ActionResult IndexPost(Guid id, int? selectedMovementStatus)
        {
            return RedirectToAction("Index", new { id, status = selectedMovementStatus });
        }
    }
}