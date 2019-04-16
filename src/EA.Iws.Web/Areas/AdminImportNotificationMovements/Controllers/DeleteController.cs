namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;
    using Requests.ImportMovement.Delete;
    using ViewModels.Delete;

    [AuthorizeActivity(UserAdministrationPermissions.CanDeleteMovements)]
    public class DeleteController : Controller
    {
        private const string MovementNumberKey = "MovementNumberKey";
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public DeleteController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var model = new IndexViewModel { NotificationId = id };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var movementId = await mediator.SendAsync(new GetImportMovementIdIfExists(id, model.Number.Value));

            if (!movementId.HasValue)
            {
                ModelState.AddModelError("Number", string.Format(DeleteControllerResources.NotExistError, model.Number));
                return View(model);
            }

            TempData[MovementNumberKey] = model.Number;
            return RedirectToAction("Delete");
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            object result;
            if (TempData.TryGetValue(MovementNumberKey, out result))
            {
                return View(new DeleteViewModel
                {
                    Number = (int)result,
                    NotificationId = id
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id, DeleteViewModel model)
        {
            var movementId = await mediator.SendAsync(new GetImportMovementIdIfExists(id, model.Number.Value));
            bool result = false;

            if (movementId.HasValue)
            {
                result = await mediator.SendAsync(new DeleteMovement(movementId.Value));

                await this.auditService.AddImportMovementAudit(this.mediator,
                    id, model.Number.Value,
                    User.GetUserId(),
                    MovementAuditType.Deleted);
            }

            var confirmModel = new ConfirmViewModel
            {
                Number = model.Number,
                Success = result,
                NotificationId = id
            };

            return RedirectToAction("Confirm", confirmModel);
        }

        [HttpGet]
        public ActionResult Confirm(ConfirmViewModel model)
        {
            return View(model);
        }
    }
}