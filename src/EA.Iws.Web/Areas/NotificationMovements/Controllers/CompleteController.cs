namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;
    using ViewModels.Complete;

    [Authorize]
    public class CompleteController : Controller
    {
        private readonly IMediator mediator;

        public CompleteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var result =
                await
                    mediator.SendAsync(new GetReceivedMovements(notificationId));

            return View(new CompleteMovementViewModel(notificationId, result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid notificationId, CompleteMovementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Date", "Complete",
                new
                {
                    id = model.RadioButtons.SelectedValue,
                    area = "Movement"
                });
        }
    }
}