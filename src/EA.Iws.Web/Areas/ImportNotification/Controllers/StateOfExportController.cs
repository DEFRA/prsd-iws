namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.Shared;
    using Requests.TransportRoute;
    using ViewModels.StateOfExport;

    [Authorize(Roles = "internal")]
    public class StateOfExportController : Controller
    {
        private readonly IMediator mediator;

        public StateOfExportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var stateOfExport = await mediator.SendAsync(new GetDraftData<StateOfExport>(id));

            var model = new StateOfExportViewModel(stateOfExport);

            model.Countries = await mediator.SendAsync(new GetCountries());

            if (model.IsCountrySelected)
            {
                var data =
                    await
                        mediator.SendAsync(
                            new GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId(model.CountryId.Value));

                model.CompetentAuthorities = data.CompetentAuthorities;
                model.ExitPoints = data.EntryOrExitPoints;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, StateOfExportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var stateOfExport = new StateOfExport
            {
                CompetentAuthorityId = model.CompetentAuthorityId,
                CountryId = model.CountryId,
                ExitPointId = model.ExitPointId
            };

            await mediator.SendAsync(new SetDraftData<StateOfExport>(id, stateOfExport));

            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [HttpGet]
        public async Task<ActionResult> GetAuthoritiesAndExitPoints(Guid countryId)
        {
            var data = await mediator.SendAsync(new GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId(countryId));

            var model = new StateOfExportViewModel
            {
                CompetentAuthorities = data.CompetentAuthorities,
                ExitPoints = data.EntryOrExitPoints
            };

            return PartialView("_CompetentAuthoritiesAndExitPoints", model);
        } 
    }
}