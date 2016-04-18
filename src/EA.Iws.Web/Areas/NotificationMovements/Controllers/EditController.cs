namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Edit;
    using ViewModels.Edit;

    [AuthorizeActivity(typeof(GetEditableMovements))]
    public class EditController : Controller
    {
        private readonly IMediator mediator;

        public EditController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var submittedMovements = await mediator.SendAsync(new GetEditableMovements(notificationId));

            var model = new EditViewModel(submittedMovements);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid notificationId, EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Index", "EditDate", new { area = "ExportMovement", id = model.Shipments.SelectedValue });
        }
    }
}