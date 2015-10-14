namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.MovementOperationReceipt;
    using ViewModels;

    public class DateCompleteController : Controller
    {
        private readonly IMediator mediator;

        public DateCompleteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var data = await mediator.SendAsync(new GetMovementOperationReceiptDataByMovementId(id));
            var model = new DateCompleteViewModel(data.DateCompleted, data.NotificationType);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, DateCompleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new CreateMovementOperationReceiptForMovement(id, model.GetDateComplete()));
            return RedirectToAction("Index", "OperationComplete");
        }
    }
}