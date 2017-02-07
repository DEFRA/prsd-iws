namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;
    using ViewModels.FinancialGuaranteeDates;

    [AuthorizeActivity(typeof(SetCurrentFinancialGuaranteeDates))]
    public class FinancialGuaranteeDatesController : Controller
    {
        private readonly IMediator mediator;

        public FinancialGuaranteeDatesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(new GetCurrentFinancialGuaranteeDetails(id));

            var model = result == null ? new UpdateFinancialGuaranteeDatesViewModel() : new UpdateFinancialGuaranteeDatesViewModel(result);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, UpdateFinancialGuaranteeDatesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = new SetCurrentFinancialGuaranteeDates(id, model.ReceivedDate.AsDateTime(),
                model.CompletedDate.AsDateTime(), model.DecisionDate.AsDateTime());

            await mediator.SendAsync(request);

            return RedirectToAction("Index", "FinancialGuaranteeDecisionHistory");
        }
    }
}