namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;
    using ViewModels;

    public class DateReceivedController : Controller
    {
        private readonly IMediator mediator;

        public DateReceivedController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var dates = await mediator.SendAsync(new GetMovementReceiptDateByMovementId(id));

            var viewModel = new DateReceivedViewModel(dates.DateReceived, dates.MovementDate);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, DateReceivedViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            await mediator.SendAsync(new CreateMovementReceiptForMovement(id, viewModel.GetDateReceived()));
            return RedirectToAction("Index", "Acceptance", new { id });
        }
    }
}