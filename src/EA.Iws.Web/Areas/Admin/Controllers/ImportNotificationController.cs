namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using ViewModels.ImportNotification;

    [Authorize]
    public class ImportNotificationController : Controller
    {
        private const string ImportNotificationNumber = "ImportNotificationNumber";

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(NotificationNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData.Add(ImportNotificationNumber, model.NotificationNumber);
            return View(model);
        }
    }
}