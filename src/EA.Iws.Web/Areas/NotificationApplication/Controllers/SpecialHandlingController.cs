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
    using ViewModels.SpecialHandling;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class SpecialHandlingController : Controller
    {
        private readonly IMediator mediator;

        public SpecialHandlingController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            var model = new SpecialHandlingViewModel { NotificationId = id };

            var specialHandlingData =
                await mediator.SendAsync(new GetSpecialHandingForNotification(id));

            if (specialHandlingData.HasSpecialHandlingRequirements.HasValue)
            {
                model.HasSpecialHandlingRequirements = specialHandlingData.HasSpecialHandlingRequirements;
                model.SpecialHandlingDetails = specialHandlingData.SpecialHandlingDetails;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(SpecialHandlingViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await
                mediator.SendAsync(new SetSpecialHandling(model.NotificationId, model.HasSpecialHandlingRequirements.GetValueOrDefault(),
                        model.SpecialHandlingDetails));

                if (backToOverview.GetValueOrDefault())
                {
                    return RedirectToAction("Index", "Home", new { id = model.NotificationId });
                }
                return RedirectToAction("Index", "StateOfExport", new { id = model.NotificationId });
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