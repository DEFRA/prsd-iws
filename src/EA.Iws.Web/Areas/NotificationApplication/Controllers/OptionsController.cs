namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;
    using ViewModels.Options;

    [AuthorizeActivity(ExportMovementPermissions.CanEditExportMovementsExternal)]
    public class OptionsController : Controller
    {
        private readonly IMediator mediator;

        public OptionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, int? status, int page = 1)
        {
            var movementsSummary = await mediator.SendAsync(new GetSummaryAndTable(id, (MovementStatus?)status, page));

            var model = new NotificationOptionsViewModel(id, movementsSummary);
            model.SelectedMovementStatus = (MovementStatus?)status;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public ActionResult IndexPost(Guid id, int? selectedMovementStatus)
        {
            return RedirectToAction("Index", new { id, status = selectedMovementStatus, page = 1 });
        }

        [HttpGet]
        public ActionResult DownloadUnavailable(Guid id)
        {
            var model = new UnavailableViewModel { NotificationId = id };

            return View(model);
        }

        [HttpGet]
        public ActionResult PostageLabelUnavailable(Guid id)
        {
            var model = new UnavailableViewModel { NotificationId = id };

            return View(model);
        }

        [HttpGet]
        public ActionResult FinancialGuaranteeUnavailable(Guid id)
        {
            var model = new UnavailableViewModel { NotificationId = id };

            return View(model);
        }

        [HttpGet]
        public ActionResult AnnexUploadUnavailable(Guid id)
        {
            var model = new UnavailableViewModel { NotificationId = id };

            return View(model);
        }
    }
}