namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.TransportRoute;
    using ViewModels.StateOfImport;

    [Authorize(Roles = "internal")]
    public class StateOfImportController : Controller
    {
        private readonly IMediator mediator;

        public StateOfImportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var stateOfImport = await mediator.SendAsync(new GetDraftData<StateOfImport>(id));

            var lookupData = await mediator.SendAsync(new GetUnitedKingdomCompetentAuthoritiesAndEntryOrExitPoints());

            var model = new StateOfImportViewModel(stateOfImport);

            if (!stateOfImport.CompetentAuthorityId.HasValue)
            {
                model.CompetentAuthorityId =
                    (await mediator.SendAsync(new GetInternalUserCompetentAuthority())).Item2.Id;
            }

            model.CompetentAuthorities = lookupData.CompetentAuthorities;
            model.EntryPoints = lookupData.EntryOrExitPoints;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, StateOfImportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var stateOfImport = new StateOfImport(id)
            {
                CompetentAuthorityId = model.CompetentAuthorityId,
                EntryPointId = model.EntryPointId
            };

            await mediator.SendAsync(new SetDraftData<StateOfImport>(id, stateOfImport));

            return RedirectToAction("Index", "TransitState");
        } 
    }
}