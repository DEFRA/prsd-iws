namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using System.Web.Mvc;

    public class RejectedMovementController : Controller
    {
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }
    }
}