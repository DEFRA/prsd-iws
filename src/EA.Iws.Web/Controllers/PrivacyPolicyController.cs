namespace EA.Iws.Web.Controllers
{
    using System.Web.Mvc;

    public class PrivacyPolicyController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}