namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
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