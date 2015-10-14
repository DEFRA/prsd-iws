namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;
    using ViewModels.Quantity;

    public class QuantityReceivedController : Controller
    {
        private readonly IMediator mediator;

        public QuantityReceivedController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(new GetMovementReceiptQuantityByMovementId(id));

            return View(new QuantityReceivedViewModel
            {
                Unit = result.Unit,
                Quantity = result.Quantity
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, QuantityReceivedViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new SetMovementReceiptQuantityByMovementId(id,
                model.Quantity.GetValueOrDefault()));

            return RedirectToAction("Index", "ReceiptComplete", new { id });
        }
    }
}