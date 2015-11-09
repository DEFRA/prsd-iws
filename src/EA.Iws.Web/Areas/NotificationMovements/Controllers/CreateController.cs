namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Web.Mvc;
    using ViewModels.Create;

    public class CreateController : Controller
    {
        private const string CreateMovementKey = "CreateMovementKey";

        [HttpGet]
        public ActionResult Index(Guid notificationId)
        {
            return View(new CreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid notificationId, CreateViewModel model)
        {
            TempData.Add(CreateMovementKey, model.NumberToCreate);

            return HttpNotFound();
        }
    }
}