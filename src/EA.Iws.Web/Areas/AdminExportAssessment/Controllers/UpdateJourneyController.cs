namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using Requests.TransportRoute;
    using ViewModels.UpdateJourney;
    using Web.ViewModels.Shared;

    [AuthorizeActivity(typeof(SetEntryPoint))]
    [AuthorizeActivity(typeof(SetExitPoint))]
    [AuthorizeActivity(typeof(AddTransitState))]
    public class UpdateJourneyController : Controller
    {
        private readonly IMediator mediator;

        public UpdateJourneyController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> EntryPoint(Guid id)
        {
            var stateOfImport = await mediator.SendAsync(new GetStateOfImportData(id));
            var entryPoints = await mediator.SendAsync(new GetEntryOrExitPointsByCountry(stateOfImport.Country.Id));

            var model = new EntryPointViewModel(stateOfImport, entryPoints);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EntryPoint(Guid id, EntryPointViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await EntryPoint(id);
            }

            await mediator.SendAsync(new SetEntryPoint(id, model.SelectedEntryPoint.Value));

            return RedirectToAction("EntryPointChanged");
        }

        [HttpGet]
        public async Task<ActionResult> EntryPointChanged(Guid id)
        {
            var stateOfImport = await mediator.SendAsync(new GetStateOfImportData(id));

            ViewBag.EntryPoint = stateOfImport.EntryPoint.Name;

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ExitPoint(Guid id)
        {
            var stateOfExport = await mediator.SendAsync(new GetStateOfExportData(id));
            var entryPoints = await mediator.SendAsync(new GetEntryOrExitPointsByCountry(stateOfExport.Country.Id));

            var model = new ExitPointViewModel(stateOfExport, entryPoints);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExitPoint(Guid id, ExitPointViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await ExitPoint(id);
            }

            await mediator.SendAsync(new SetExitPoint(id, model.SelectedExitPoint.Value));

            return RedirectToAction("ExitPointChanged");
        }

        [HttpGet]
        public async Task<ActionResult> ExitPointChanged(Guid id)
        {
            var stateOfImport = await mediator.SendAsync(new GetStateOfExportData(id));

            ViewBag.ExitPoint = stateOfImport.ExitPoint.Name;

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> AddTransitState(Guid id)
        {
            var countries = await mediator.SendAsync(new GetAllCountriesHavingCompetentAuthorities());

            var model = new AddTransitStateViewModel
            {
                Countries = new SelectList(countries, "Id", "Name")
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetTransitAuthoritiesAndEntryOrExitPointsByCountryId(Guid countryId)
        {
            var model = new AddTransitStateViewModel();

            var result = await mediator.SendAsync(new GetTransitAuthoritiesAndEntryOrExitPointsByCountryId(countryId));

            var competentAuthoritiesKeyValuePairs = result.CompetentAuthorities.Select(ca =>
                new KeyValuePair<string, Guid>(ca.Code + " - " + ca.Name, ca.Id));
            var competentAuthorityRadioButtons = new StringGuidRadioButtons(competentAuthoritiesKeyValuePairs);

            model.CompetentAuthorities = competentAuthorityRadioButtons;
            model.EntryOrExitPoints = new SelectList(result.EntryOrExitPoints, "Id", "Name");

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddTransitState(Guid id, AddTransitStateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var countries = await mediator.SendAsync(new GetAllCountriesHavingCompetentAuthorities());
                model.Countries = new SelectList(countries, "Id", "Name");

                var result = await mediator.SendAsync(new GetTransitAuthoritiesAndEntryOrExitPointsByCountryId(model.CountryId.Value));

                var competentAuthoritiesKeyValuePairs = result.CompetentAuthorities.Select(ca =>
                    new KeyValuePair<string, Guid>(ca.Code + " - " + ca.Name, ca.Id));
                var competentAuthorityRadioButtons = new StringGuidRadioButtons(competentAuthoritiesKeyValuePairs);

                model.CompetentAuthorities = competentAuthorityRadioButtons;
                model.EntryOrExitPoints = new SelectList(result.EntryOrExitPoints, "Id", "Name");

                return View(model);
            }

            await
                mediator.SendAsync(new AddTransitState(id, model.CountryId.Value, model.EntryPointId.Value,
                    model.ExitPointId.Value, model.CompetentAuthorities.SelectedValue));

            return RedirectToAction("Index", "Overview");
        }
    }
}