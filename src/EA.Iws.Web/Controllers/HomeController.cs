namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Mvc;
    using Infrastructure;
    using ViewModels.Shared;

    public class HomeController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInternalUser())
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

                return RedirectToAction(actionName: "Home", controllerName: "Applicant");
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LandingPage()
        {
            var model = new YesNoChoiceViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LandingPage(YesNoChoiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Choices.SelectedValue.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
            {
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("ApplicantRegistration", "Registration");
        }

        [AllowAnonymous]
        public ActionResult _IwsTitle()
        {
            var identity = (ClaimsIdentity)User.Identity;

            ViewBag.Name = identity.HasClaim(c => c.Type.Equals(ClaimTypes.Name)) ? identity.Claims.Single(c => c.Type.Equals(ClaimTypes.Name)).Value : string.Empty;

            return PartialView();
        }
    }
}