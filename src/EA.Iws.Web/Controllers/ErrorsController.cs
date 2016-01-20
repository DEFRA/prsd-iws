namespace EA.Iws.Web.Controllers
{
    using System.Web.Mvc;

    [AllowAnonymous]
    public class ErrorsController : Controller
    {
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InternalError()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public ActionResult VirusDetected()
        {
            return View();
        }
    }
}