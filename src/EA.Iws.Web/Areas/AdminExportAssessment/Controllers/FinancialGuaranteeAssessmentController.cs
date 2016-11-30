namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;
    using ViewModels.FinancialGuaranteeAssessment;

    [Authorize(Roles = "internal")]
    public class FinancialGuaranteeAssessmentController : Controller
    {
        private IMediator mediator;

        public FinancialGuaranteeAssessmentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var currentFinancialGuarantee = await mediator.SendAsync(new GetCurrentFinancialGuaranteeDetails(id));

            return View(currentFinancialGuarantee);
        }

        [HttpGet]
        public ActionResult New(Guid id)
        {
            return View(new NewFinancialGuaranteeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> New(Guid id, NewFinancialGuaranteeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await Task.Yield();

            return View(model);
        }
    }
}