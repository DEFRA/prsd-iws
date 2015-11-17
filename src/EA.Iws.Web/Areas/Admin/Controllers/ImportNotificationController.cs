namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using ViewModels.ImportNotification;

    [Authorize]
    public class ImportNotificationController : Controller
    {
        private readonly IMediator mediator;
        private const string ImportNotificationNumber = "ImportNotificationNumber";

        public ImportNotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Number()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Number(NotificationNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await mediator.SendAsync(new CheckImportNumberUnique(model.NotificationNumber)))
            {
                ModelState.AddModelError("NotificationNumber", "A record for this notification number already exists");
                return View(model);
            }

            TempData[ImportNotificationNumber] = model.NotificationNumber;
            return RedirectToAction("NotificationType");
        }

        [HttpGet]
        public ActionResult NotificationType()
        {
            object number;
            if (TempData.TryGetValue(ImportNotificationNumber, out number))
            {
                return View(new NotificationTypeViewModel
                {
                    NotificationNumber = number.ToString()
                });
            }

            return RedirectToAction("Number");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NotificationType(NotificationTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var id = await mediator.SendAsync(new CreateImportNotification(model.NotificationNumber,
                (NotificationType)model.NotificationTypeRadioButtons.SelectedValue));

            return RedirectToAction("Index", "Preconsented", new { area = "ImportNotification", id });
        }
    }
}