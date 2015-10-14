namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.MovementReceipt;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;
    using ViewModels.Acceptance;

    [Authorize]
    public class AcceptanceController : Controller
    {
        private readonly IMediator mediator;

        public AcceptanceController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var acceptanceInfo = await mediator.SendAsync(new GetMovementAcceptanceDataByMovementId(id));
            var model = new AcceptanceViewModel(acceptanceInfo);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, AcceptanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new UpdateShipmentAcceptanceDataByMovementId(id, model.Decision.GetValueOrDefault(), model.RejectReason));

            if (model.Decision == Decision.Rejected)
            {
                // Change to redirect to completed page when it is available
                return RedirectToAction("Index", "ReceiptComplete", new { id });
            }

            return RedirectToAction("Index", "QuantityReceived", new { id });
        }
    }
}