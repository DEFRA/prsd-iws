namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    [Authorize(Roles = "internal")]
    public class FinancialGuaranteeAssessmentController : Controller
    {
        [HttpGet]
        public ActionResult Index(Guid id)
        {
            return View();
        }
    }
}