namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    [AuthorizeActivity(typeof(GetFinancialGuaranteeDataByNotificationApplicationId))]
    public class FinancialGuaranteeDetailsController : Controller
    {
        private readonly IMediator mediator;

        public FinancialGuaranteeDetailsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, Guid financialGuaranteeId)
        {
            var financialGuarantee = await mediator.SendAsync(
                new GetFinancialGuaranteeDataByNotificationApplicationId(id, financialGuaranteeId));

            return View(financialGuarantee);
        }
    }
}