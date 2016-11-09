namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
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

        public DeleteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            return View();
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

            if (movementId.HasValue)
            {
                TempData[MovementNumberKey] = model.Number;
                return RedirectToAction("Delete");
            }

            return RedirectToAction("Index");
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