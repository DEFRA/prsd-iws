namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.OperationCodes;

    public class OperationCodesController : Controller
    {
        public ActionResult Edit()
        {
            OperationCodesViewModel model = new OperationCodesViewModel();

            return View(model);
        }
    }
}