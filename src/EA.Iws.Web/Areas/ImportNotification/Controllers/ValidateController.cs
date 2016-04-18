namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.ImportNotification.Validate;
    using ViewModels.Validate;

    [Authorize(Roles = "internal")]
    public class ValidateController : Controller
    {
        private readonly IMediator mediator;

        public ValidateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var results = await mediator.SendAsync(new ValidateImportNotification(id));

            var model = new ValidateViewModel(results);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(Guid id)
        {
            var result = await mediator.SendAsync(new CompleteDraftImportNotification(id));

            if (result)
            {
                return RedirectToAction("Index", "Complete");
            }

            return RedirectToAction("Error", "Complete");
        }
    }
}