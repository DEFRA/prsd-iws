namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.Notification.Audit;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.WasteType;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.WasteGenerationProcess;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class WasteGenerationProcessController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public WasteGenerationProcessController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
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
                var wasteGenerationProcessData = await mediator.SendAsync(new GetWasteGenerationProcess(model.NotificationId));

                await mediator.SendAsync(model.ToRequest());

                await this.auditService.AddAuditEntry(this.mediator,
                   model.NotificationId,
                   User.GetUserId(),
                   wasteGenerationProcessData.Process == null ? NotificationAuditType.Added : NotificationAuditType.Updated,
                   NotificationAuditScreenType.ProcessOfGeneration);
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