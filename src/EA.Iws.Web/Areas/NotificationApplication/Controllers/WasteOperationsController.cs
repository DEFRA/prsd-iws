namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.OperationCodes;
    using Requests.TechnologyEmployed;
    using ViewModels.WasteOperations;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class WasteOperationsController : Controller
    {
        private readonly IMediator mediator;

        public WasteOperationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> OperationCodes(Guid id, bool? backToOverview = null)
        {
            var notificationType =
                (await mediator.SendAsync(new GetNotificationBasicInfo(id))).NotificationType;

            return notificationType == NotificationType.Disposal ?
                RedirectToAction("DisposalCodes", "WasteOperations", new { id, backToOverview })
                : RedirectToAction("RecoveryCodes", "WasteOperations", new { id, backToOverview });
        }

        [HttpGet]
        public async Task<ActionResult> RecoveryCodes(Guid id)
        {
            var selectedCodes =
                    await mediator.SendAsync(new GetOperationCodesByNotificationId(id));

            var model = new OperationCodesViewModel(NotificationType.Recovery, selectedCodes);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoveryCodes(Guid id, OperationCodesViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new AddRecoveryCodes(id, model.SelectedValues));

            return backToOverview.GetValueOrDefault() ? 
                RedirectToAction("Index", "Home")
                : RedirectToAction("TechnologyEmployed", "WasteOperations");
        }

        [HttpGet]
        public async Task<ActionResult> DisposalCodes(Guid id, bool? backToOverview = null)
        {
            var selectedCodes =
                    await mediator.SendAsync(new GetOperationCodesByNotificationId(id));

            var model = new OperationCodesViewModel(NotificationType.Disposal, selectedCodes);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisposalCodes(Guid id, OperationCodesViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new AddDisposalCodes(id, model.SelectedValues));

            return backToOverview.GetValueOrDefault() ?
                RedirectToAction("Index", "Home")
                : RedirectToAction("TechnologyEmployed", "WasteOperations");
        }

        [HttpGet]
        public async Task<ActionResult> TechnologyEmployed(Guid id, bool? backToOverview = null)
        {
            var technologyEmployedData =
                await mediator.SendAsync(new GetTechnologyEmployed(id));

            return View(new TechnologyEmployedViewModel(id, technologyEmployedData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TechnologyEmployed(TechnologyEmployedViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await
                mediator.SendAsync(new SetTechnologyEmployed(model.NotificationId, model.AnnexProvided, model.Details, model.FurtherDetails));

            return backToOverview.GetValueOrDefault() ? 
                RedirectToAction("Index", "Home")
                : RedirectToAction("Index", "ReasonForExport");
        }
    }
}