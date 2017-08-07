namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels.UploadChoice;

    [AuthorizeActivity(typeof(GetNewMovementIdsByNotificationId))]
    public class UploadChoiceController : Controller
    {
        private readonly IMediator mediator;

        public UploadChoiceController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var result = await mediator.SendAsync(new GetNewMovementIdsByNotificationId(notificationId));

            return View(new UploadChoiceViewModel(notificationId, result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid notificationId, UploadChoiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Index", "Upload", model.Shipments.Where(s => s.IsSelected).Select(s => s.Id).ToRouteValueDictionary("movementIds"));
        }
    }
}