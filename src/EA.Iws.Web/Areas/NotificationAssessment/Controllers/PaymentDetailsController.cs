namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
{
    using System;
    using System.Web.Mvc;
    using ViewModels.PaymentDetails;

    [Authorize(Roles = "internal")]
    public class PaymentDetailsController : Controller
    {
        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var model = new PaymentDetailsViewModel { NotificationId = id };

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(PaymentDetailsViewModel model)
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