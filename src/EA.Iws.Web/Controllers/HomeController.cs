namespace EA.Iws.Web.Controllers
{
    using System.Web.Mvc;
    using ViewModels.Home;

    public class HomeController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(actionName: "Home", controllerName: "Applicant");
            }

            return View("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LandingPage()
        {
            var model = new YesNoChoice();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LandingPage(YesNoChoice model)
        {
            if (!ModelState.IsValid)
            {
                return View("LandingPage", model);
            }

            if (model.Choices.SelectedValue == "No")
            {
                return RedirectToAction("ApplicantRegistration", "Registration");
            }

            return RedirectToAction("Login", "Account");
        }
    }
}