namespace EA.Iws.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Mvc;
    using Infrastructure;

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

            return RedirectToAction("Login", "Account");
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