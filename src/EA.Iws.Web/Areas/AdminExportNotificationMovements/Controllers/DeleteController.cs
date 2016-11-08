namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Capture;
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
            var model = new IndexViewModel();

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

            var movementId = await mediator.SendAsync(new GetMovementIdIfExists(id, model.Number.Value));

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
        public ActionResult Delete(Guid id, DeleteViewModel model)
        {
            //If model is valid delete the shipment

            //Then redirect to confirmation screen and display success or failure
            var confirmModel = new ConfirmViewModel();
            confirmModel.Number = model.Number;
            confirmModel.Success = false;
            confirmModel.NotificationId = id;

            return RedirectToAction("Confirm", confirmModel);
        }

        [HttpGet]
        public ActionResult Confirm(ConfirmViewModel model)
        {
            return View(model);
        }
    }
}