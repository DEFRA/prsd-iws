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
    using Prsd.Core.Web.ApiClient;
    using Requests.Shared;
    using Requests.TransitState;
    using Requests.TransportRoute;
    using ViewModels.TransitState;
    using Web.ViewModels.Shared;

    [Authorize]
    public class TransitStateController : Controller
    {
        private readonly Func<IIwsClient> apiClient;
        private readonly IMap<TransitStateWithTransportRouteData, TransitStateViewModel> transitStateMapper;

        private const string SelectCountry = "country";
        private const string Submit = "submit";
        private const string ChangeCountry = "changeCountry";

        public TransitStateController(Func<IIwsClient> apiClient, IMap<TransitStateWithTransportRouteData, TransitStateViewModel> transitStateMapper)
        {
            this.apiClient = apiClient;
            this.transitStateMapper = transitStateMapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, Guid? entityId)
        {
            using (var client = apiClient())
            {
                TransitStateWithTransportRouteData result =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new GetTransitStateWithTransportRouteDataByNotificationId(id, entityId));

                var model = transitStateMapper.Map(result);

                return View(model);
            }
        }

        public async Task<ActionResult> Index(Guid id, Guid? entityId, TransitStateViewModel model, string submit)
        {
            using (var client = apiClient())
            {
                model.Countries = await GetCountrySelectListForModel(client, model);
                await GetCompetentAuthoritiesAndCountriesForModel(client, model);

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
                        return await SubmitAction(id, entityId, model, client);
                }
            }
        }

        private async Task<ActionResult> SelectCountryAction(Guid id, TransitStateViewModel model, IIwsClient client)
        {
            model.ShowNextSection = true;

            return View("Index", model);
        }

        private ActionResult ChangeCountryAction(Guid id, TransitStateViewModel model, IIwsClient client)
        {
            ModelState.Clear();
            model.ShowNextSection = false;

            return View("Index", model);
        }

        private async Task<ActionResult> SubmitAction(Guid id, Guid? transitStateId, TransitStateViewModel model, IIwsClient client)
        {
            try
            {
                var request = new SetTransitStateForNotification(id,
                    model.CountryId.Value,
                    model.EntryPointId.Value,
                    model.ExitPointId.Value,
                    model.CompetentAuthorities.SelectedValue,
                    transitStateId,
                    model.OrdinalPosition);
                
                await client.SendAsync(User.GetAccessToken(), request);

                return RedirectToAction("Summary", "TransportRoute", new { id });
            }
            catch (ApiException)
            {
                ModelState.AddModelError(string.Empty, "Error saving this record. You may already have saved this record, return to the summary to edit this record.");
            }
            return View(model);
        }

        private async Task GetCompetentAuthoritiesAndCountriesForModel(IIwsClient client, TransitStateViewModel model)
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
            model.EntryOrExitPoints = new SelectList(entryPointsAndCompetentAuthorities.EntryOrExitPoints, "Id", "Name");
        }

        private async Task<SelectList> GetCountrySelectListForModel(IIwsClient client, TransitStateViewModel model)
        {
            var countries = await client.SendAsync(new GetCountries());

            return (model.CountryId.HasValue)
                ? new SelectList(countries, "Id", "Name", model.CountryId.Value)
                : new SelectList(countries, "Id", "Name");
        }
    }
}