namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
{
    using System;
    using System.Web.Mvc;
    using ViewModels.RefundDetails;

    [Authorize(Roles = "internal")]
    public class RefundDetailsController : Controller
    {
        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var model = new RefundDetailsViewModel { NotificationId = id };

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(RefundDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // @todo save the data

            return RedirectToAction("index", "Home", new { id = model.NotificationId });
        }
    }
}