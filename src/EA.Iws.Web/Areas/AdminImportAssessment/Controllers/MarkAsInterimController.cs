namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Web.Mvc;

    [Authorize(Roles = "internal")]
    public class MarkAsInterimController : Controller
    {
        [HttpGet]
        public ActionResult Index(Guid id)
        {
            return View();
        }
    }
}