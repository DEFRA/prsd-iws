namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Requests.Shared;
    using Requests.StateOfImport;
    using Requests.TransportRoute;
    using ViewModels.StateOfImport;
    using Web.ViewModels.Shared;

    [Authorize]
    public class StateOfImportController : Controller
    {
        private readonly Func<IIwsClient> apiClient;
        private readonly IMap<StateOfImportWithTransportRouteData, StateOfImportViewModel> mapper;

        private const string SelectCountry = "country";
        private const string Submit = "submit";
        private const string ChangeCountry = "changeCountry";

        public StateOfImportController(Func<IIwsClient> apiClient, IMap<StateOfImportWithTransportRouteData, StateOfImportViewModel> mapper)
        {
            this.apiClient = apiClient;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = apiClient())
            {
                var stateOfImportSetData = await client.SendAsync(User.GetAccessToken(), new GetStateOfImportWithTransportRouteDataByNotificationId(id));

                var model = mapper.Map(stateOfImportSetData);

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, StateOfImportViewModel model, string submit)
        {
            using (var client = apiClient())
            {
                model.Countries = await GetCountrySelectListForModel(client, model);
                await GetCompetentAuthoritiesAndEntryPoints(client, model);

                if (!ModelState.IsValid && submit != ChangeCountry)
                {
                    return View(model);
                }

                switch (submit)
                {
                    case SelectCountry:
                        return await SelectCountryAction(id, model, client);
                    case ChangeCountry:
                        return ChangeCountryAction(id, model, client);
                    default:
                        return await SubmitAction(id, model, client);
                }
            }
        }

        private async Task<ActionResult> SelectCountryAction(Guid id, StateOfImportViewModel model, IIwsClient client)
        {
            model.ShowNextSection = true;

            return View("Index", model);
        }

        private ActionResult ChangeCountryAction(Guid id, StateOfImportViewModel model, IIwsClient client)
        {
            ModelState.Clear();
            model.ShowNextSection = false;

            return View("Index", model);
        }

        private async Task<ActionResult> SubmitAction(Guid id, StateOfImportViewModel model, IIwsClient client)
        {
                await client.SendAsync(User.GetAccessToken(),
                    new SetStateOfImportForNotification(id,
                        model.CountryId.Value,
                        model.EntryOrExitPointId.Value,
                        model.CompetentAuthorities.SelectedValue));

                return RedirectToAction("Summary", "TransportRoute", new { id });
        }

        private async Task<SelectList> GetCountrySelectListForModel(IIwsClient client, StateOfImportViewModel model)
        {
            var countries = await client.SendAsync(new GetCountries());

            return (model.CountryId.HasValue)
                ? new SelectList(countries, "Id", "Name", model.CountryId.Value)
                : new SelectList(countries, "Id", "Name");
        }

        private async Task GetCompetentAuthoritiesAndEntryPoints(IIwsClient client, StateOfImportViewModel model)
        {
            if (!model.CountryId.HasValue)
            {
                return;
            }

            var entryPointsAndCompetentAuthorities =
                await
                    client.SendAsync(User.GetAccessToken(), new GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId(model.CountryId.Value));

            var competentAuthoritiesKeyValuePairs = entryPointsAndCompetentAuthorities.CompetentAuthorities.Select(ca =>
                new KeyValuePair<string, Guid>(ca.Name, ca.Id));
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