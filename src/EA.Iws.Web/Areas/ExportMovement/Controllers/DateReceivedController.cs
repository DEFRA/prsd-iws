namespace EA.Iws.Web.Areas.ExportMovement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels;

    [AuthorizeActivity(typeof(GetMovementDateByMovementId))]
    public class DateReceivedController : Controller
    {
        private readonly IMediator mediator;
        private const string DateReceivedKey = "DateReceived";

        public DateReceivedController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var movementDate = await mediator.SendAsync(new GetMovementDateByMovementId(id));

            var viewModel = new DateReceivedViewModel(movementDate);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid id, DateReceivedViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            TempData[DateReceivedKey] = viewModel.GetDateReceived();

            return RedirectToAction("Index", "QuantityReceived", new { id });
        }
    }
}