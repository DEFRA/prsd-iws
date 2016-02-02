namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.FinancialGuarantee;
    using ViewModels.FinancialGuaranteeDates;

    [Authorize(Roles = "internal")]
    public class FinancialGuaranteeDatesController : Controller
    {
        private readonly IMediator mediator;

        public FinancialGuaranteeDatesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> ReceivedDate(Guid id)
        {
            var receivedDate = await mediator.SendAsync(new GetReceivedDate(id));

            if (!receivedDate.HasValue)
            {
                return View(new ReceivedDateViewModel());
            }

            return RedirectToAction("ReceivedAndCompletedDate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReceivedDate(Guid id, ReceivedDateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new CreateFinancialGuarantee(id, model.ReceivedDate.AsDateTime().Value));

            return RedirectToAction("ReceivedAndCompletedDate");
        } 

        [HttpGet]
        public async Task<ActionResult> ReceivedAndCompletedDate(Guid id)
        {
            var dates = await mediator.SendAsync(new GetReceivedAndCompletedDate(id));

            if (!dates.ReceivedDate.HasValue)
            {
                return RedirectToAction("ReceivedDate");
            }

            return View(new ReceivedAndCompletedDateViewModel(dates));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReceivedAndCompletedDate(Guid id, ReceivedAndCompletedDateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.IsReceivedDateChanged)
            {
               await mediator.SendAsync(new SetReceivedAndCompletedDate(id, 
                   model.CompletedDate.AsDateTime().Value, 
                   model.ReceivedDate.AsDateTime().Value));
            }
            else
            {
                await mediator.SendAsync(new SetCompletedDate(id, model.CompletedDate.AsDateTime().Value));
            }

            return RedirectToAction("Index", "KeyDates");
        } 
    }
}