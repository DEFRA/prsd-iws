namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.WasteType;
    using ViewModels.WasteGenerationProcess;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class WasteGenerationProcessController : Controller
    {
        private readonly IMediator mediator;

        public WasteGenerationProcessController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            var wasteGenerationProcessData =
                await mediator.SendAsync(new GetWasteGenerationProcess(id));

            var model = new WasteGenerationProcessViewModel(wasteGenerationProcessData);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(WasteGenerationProcessViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await mediator.SendAsync(model.ToRequest());
                if (backToOverview.GetValueOrDefault())
                {
                    return RedirectToAction("Index", "Home", new { id = model.NotificationId });
                }

                return RedirectToAction("Index", "PhysicalCharacteristics", new { id = model.NotificationId });
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);
                if (ModelState.IsValid)
                {
                    throw;
                }
            }
            return View(model);
        }
    }
}