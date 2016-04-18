namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System.Web.Mvc;

    [Authorize(Roles = "internal")]
    public class CompleteController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }
    }
}