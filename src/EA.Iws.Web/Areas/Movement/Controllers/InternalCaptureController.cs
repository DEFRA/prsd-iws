namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels.InternalCapture;

    [Authorize(Roles = "internal")]
    public class InternalCaptureController : Controller
    {
        private readonly IMediator mediator;

        public InternalCaptureController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(new GetMovementReceiptAndRecoveryData(id));

            var model = new CaptureViewModel(result);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid id, CaptureViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            throw new NotImplementedException();
        }
    }
}