namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Web.Mvc;
    using Api.Client;
    using ViewModels.FinancialGuarantee;

    [Authorize(Roles = "internal")]
    public class FinancialGuaranteeController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public FinancialGuaranteeController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public ActionResult Information(Guid id)
        {
            return View(new FinancialGuaranteeInformationViewModel());
        }

        public ActionResult Information(Guid id, FinancialGuaranteeInformationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return View(new FinancialGuaranteeInformationViewModel());
        }
    }
}