namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;
    using ViewModels.Capture;

    [Authorize(Roles = "internal")]
    public class CaptureController : Controller
    {
        private readonly IMediator mediator;

        public CaptureController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, SearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await mediator.SendAsync(new GetImportMovementIdIfExists(id, model.Number.Value));

            throw new NotImplementedException();
        } 
    }
}