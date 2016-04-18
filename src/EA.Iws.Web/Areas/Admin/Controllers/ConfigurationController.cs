namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    [Authorize(Roles = "internal")]
    public class ConfigurationController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}