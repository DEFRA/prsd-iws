namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System.Web.Mvc;

    [Authorize(Roles = "internal")]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}