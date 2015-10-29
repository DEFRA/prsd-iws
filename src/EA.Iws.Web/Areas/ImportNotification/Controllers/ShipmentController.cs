namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System.Web.Mvc;
    using ViewModels;

    [Authorize(Roles = "internal")]
    public class ShipmentController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new ShipmentViewModel());
        }
    }
}