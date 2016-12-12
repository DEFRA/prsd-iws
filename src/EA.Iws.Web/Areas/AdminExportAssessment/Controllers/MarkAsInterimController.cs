namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels;

    [Authorize(Roles = "internal")]
    public class MarkAsInterimController : Controller
    {
        private readonly AuthorizationService authorizationService;
        private readonly IMediator mediator;

        public MarkAsInterimController(IMediator mediator, AuthorizationService authorizationService)
        {
            this.mediator = mediator;
            this.authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var interimStatus = await mediator.SendAsync(new GetInterimStatus(id));

            var model = new MarkAsInterimViewModel(interimStatus);

            model.CanUpdateInterimStatus = await authorizationService.AuthorizeActivity(UserAdministrationPermissions.CanUpdateInterimStatus);

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

            await mediator.SendAsync(new MarkAsInterim(model.NotificationId, model.IsInterim.Value));

            return RedirectToAction("Index", "KeyDates");
        }
    }
}