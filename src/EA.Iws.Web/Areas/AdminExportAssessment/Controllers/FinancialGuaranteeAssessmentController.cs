namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

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
    }
}