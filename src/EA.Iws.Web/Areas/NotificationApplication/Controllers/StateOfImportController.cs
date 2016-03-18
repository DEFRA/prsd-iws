namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.Shared;
    using Requests.StateOfImport;
    using Requests.TransportRoute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.StateOfImport;
    using Web.ViewModels.Shared;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class StateOfImportController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMap<StateOfImportWithTransportRouteData, StateOfImportViewModel> mapper;

        private const string SelectCountry = "country";
        private const string ChangeCountry = "changeCountry";

        public StateOfImportController(IMediator mediator, IMap<StateOfImportWithTransportRouteData, StateOfImportViewModel> mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            var stateOfImportSetData = await mediator.SendAsync(new GetStateOfImportWithTransportRouteDataByNotificationId(id));

            var notificationCompetentAuthority = await mediator.SendAsync(new GetUnitedKingdomCompetentAuthorityByNotificationId(id));

            var model = mapper.Map(stateOfImportSetData);
            model.NotificationCompetentAuthority = notificationCompetentAuthority.AsUKCompetantAuthority();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, StateOfImportViewModel model, string submit, bool? backToOverview = null)
        {
            model.Countries = await GetCountrySelectListForModel(model);
            await GetCompetentAuthoritiesAndEntryPoints(model);

            if (!ModelState.IsValid && submit != ChangeCountry)
            {
                return View(model);
            }

            switch (submit)
            {
                case SelectCountry:
                    return SelectCountryAction(model);
                case ChangeCountry:
                    return ChangeCountryAction(model);
                default:
                    return await SubmitAction(id, model, backToOverview);
            }
        }

        private ActionResult SelectCountryAction(StateOfImportViewModel model)
        {
            model.ShowNextSection = true;

            return View("Index", model);
        }

        private ActionResult ChangeCountryAction(StateOfImportViewModel model)
        {
            ModelState.Clear();
            model.ShowNextSection = false;

            return View("Index", model);
        }

        private async Task<ActionResult> SubmitAction(Guid id, StateOfImportViewModel model, bool? backToOverview)
        {
            await mediator.SendAsync(new SetStateOfImportForNotification(id,
                    model.CountryId.Value,
                    model.EntryOrExitPointId.Value,
                    model.CompetentAuthorities.SelectedValue));

            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home", new { id });
            }
            else
            {
                return RedirectToAction("Summary", "TransportRoute", new { id });
            }
        }

        private async Task<SelectList> GetCountrySelectListForModel(StateOfImportViewModel model)
        {
            var countries = await mediator.SendAsync(new GetAllCountriesHavingCompetentAuthorities());

            return (model.CountryId.HasValue)
                ? new SelectList(countries, "Id", "Name", model.CountryId.Value)
                : new SelectList(countries, "Id", "Name");
        }

        private async Task GetCompetentAuthoritiesAndEntryPoints(StateOfImportViewModel model)
        {
            if (!model.CountryId.HasValue)
            {
                return;
            }

            var entryPointsAndCompetentAuthorities =
                await
                    mediator.SendAsync(new GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId(model.CountryId.Value));

            var competentAuthoritiesKeyValuePairs = entryPointsAndCompetentAuthorities.CompetentAuthorities.Select(ca =>
                new KeyValuePair<string, Guid>(ca.Code + " -" + ca.Name, ca.Id));
            var competentAuthorityRadioButtons = new StringGuidRadioButtons(competentAuthoritiesKeyValuePairs);

            if (model.CompetentAuthorities != null)
            {
                competentAuthorityRadioButtons.SelectedValue = model.CompetentAuthorities.SelectedValue;
            }

            model.CompetentAuthorities = competentAuthorityRadioButtons;
            model.EntryPoints = new SelectList(entryPointsAndCompetentAuthorities.EntryOrExitPoints, "Id", "Name");
        }
    }
}