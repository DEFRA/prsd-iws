namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.ImportNotificationAssessment;
    using ViewModels.MarkAsInterim;

    [Authorize(Roles = "internal")]
    public class MarkAsInterimController : Controller
    {
        private readonly AuthorizationService authorizationService;
        private readonly IMediator mediator;

        public MarkAsInterimController(AuthorizationService authorizationService, IMediator mediator)
        {
            this.authorizationService = authorizationService;
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var data = await mediator.SendAsync(new GetNotificationDetails(id));
            var model = new MarkAsInterimViewModel(data);

            model.IsAuthorised = await authorizationService.AuthorizeActivity(typeof(UpdateInterimStatus));

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(MarkAsInterimViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new UpdateInterimStatus(model.NotificationId, model.IsInterim.GetValueOrDefault()));

            return RedirectToAction("Index", "Home", new { area = "ImportNotification", section = ImportNavigationSection.Assessment });
        }
    }
}