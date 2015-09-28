namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System.Web.Mvc;

    [Authorize]
    public class ReadOnlyController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}