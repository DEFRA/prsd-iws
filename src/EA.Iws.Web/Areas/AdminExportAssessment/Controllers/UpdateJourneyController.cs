namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System.Web.Mvc;

    public class UpdateJourneyController : Controller
    {
        [HttpGet]
        public ActionResult EntryPoint()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ExitPoint()
        {
            return View();
        }
    }
}