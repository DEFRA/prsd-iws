namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using ViewModels.NotificationApplication;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class ReasonForExportController : Controller
    {
        private readonly IMediator mediator;

        public ReasonForExportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            var reasonForExport = await mediator.SendAsync(new GetReasonForExport(id));

            var model = new ReasonForExportViewModel
            {
                NotificationId = id,
                ReasonForExport = reasonForExport
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ReasonForExportViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await
                    mediator.SendAsync(new SetReasonForExport(model.NotificationId, model.ReasonForExport));

                if (backToOverview.GetValueOrDefault())
                {
                    return RedirectToAction("Index", "Home", new { id = model.NotificationId });
                }

                return RedirectToAction("List", "Carrier", new { id = model.NotificationId });
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