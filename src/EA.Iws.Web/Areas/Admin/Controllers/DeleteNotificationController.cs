namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Requests.NotificationAssessment;
    using ViewModels.DeleteNotification;

    [AuthorizeActivity(typeof(DeleteExportNotification))]
    public class DeleteNotificationController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var deleteModel = new DeleteViewModel();

            return RedirectToAction("Delete", deleteModel);
        }

        [HttpGet]
        public ActionResult Delete(DeleteViewModel model)
        {
            return View(model);
        }
    }
}