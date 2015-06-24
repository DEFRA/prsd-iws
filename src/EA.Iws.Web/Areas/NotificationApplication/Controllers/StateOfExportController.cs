namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.StateOfExport;
    using Core.TransportRoute;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Requests.Shared;
    using Requests.StateOfExport;
    using Requests.TransportRoute;
    using ViewModels.StateOfExport;
    using Web.ViewModels.Shared;

    [Authorize]
    public class StateOfExportController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public StateOfExportController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            using (var client = apiClient())
            {
                await this.BindCountryList(client);
            }

            return View(new StateOfExportViewModel
            {
                CountryId = Guid.Parse(((SelectList)ViewBag.Countries).SelectedValue.ToString())
            });
        }

        [HttpPost]
        public async Task<ActionResult> Add(Guid id, StateOfExportViewModel model, string selectCountry, string submit)
        {
            await this.BindCountryList(apiClient);

            if (!ModelState.IsValid && !string.IsNullOrWhiteSpace(submit))
            {
                await BindExitOrEntryPointSelectList(apiClient, model.CountryId);
                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(selectCountry))
            {
                return await StateOfExportCountrySelectedPostback(id, model);
            }

            try
            {
                using (var client = apiClient())
                {
                    await this.BindExitOrEntryPointSelectList(apiClient, model.CountryId);
                    await client.SendAsync(User.GetAccessToken(),
                        new AddStateOfExportToNotification(id, model.CountryId, model.EntryOrExitPointId.Value, model.CompetentAuthorities.SelectedValue));
                }
            }
            catch (ApiException)
            {
                ModelState.AddModelError(string.Empty, "Error saving this record.");
                return View(model);
            }

            return RedirectToAction("Index", "StateOfImport", new { id });
        }

        private async Task<ActionResult> StateOfExportCountrySelectedPostback(Guid id, StateOfExportViewModel model)
        {
            RemoveModelStateErrors();
            using (var client = apiClient())
            {
                var competentAuthorities = await client.SendAsync(new GetCompetentAuthoritiesByCountry(model.CountryId));
                var radioButtons = competentAuthorities.Select(ca => new KeyValuePair<string, Guid>(string.Format("{0} - {1}", ca.Code, ca.Name), ca.Id));
                model.CompetentAuthorities = new StringGuidRadioButtons(radioButtons);
                model.LoadNextSection = true;

                await BindExitOrEntryPointSelectList(client, model.CountryId);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id, string edit)
        {
            using (var client = apiClient())
            {
                TransportRouteData transportRoute = await client.SendAsync(User.GetAccessToken(), new GetTransportRouteSummaryForNotification(id));
                StateOfExportData stateOfExport = transportRoute.StateOfExportData;
                var countries = await GetCountrySelectList(client, stateOfExport.Country.Id);
                EditStateOfExportViewModel model = null;
                var competentAuthorities =
                await GetCompetentAuthorities(client, stateOfExport.Country.Id, stateOfExport.CompetentAuthority.Id);
                var exitPoints = await GetExitPointsSelectList(client, stateOfExport.Country.Id, stateOfExport.ExitPoint.Id);
                
                model = new EditStateOfExportViewModel
                    {
                        NotificationId = id,
                        CompetentAuthorities = competentAuthorities,
                        CompetentAuthorityId = stateOfExport.CompetentAuthority.Id,
                        Countries = countries,
                        CountryId = stateOfExport.Country.Id,
                        ExitPointId = stateOfExport.ExitPoint.Id,
                        ExitPoints = exitPoints
                    };
                // User only wants to edit the country
                if (!string.IsNullOrWhiteSpace(edit) && edit.Equals("COUNTRY", StringComparison.InvariantCultureIgnoreCase))
                {
                    model.ShowNextSection = false;
                }
                else
                {
                    model.ShowNextSection = true;
                }

                if (transportRoute.StateOfImportData != null)
                {
                    model.StateOfImportCountryId = transportRoute.StateOfImportData.Country.Id;
                }

                if (transportRoute.TransitStatesData != null)
                {
                    model.TransitStateCompetentAuthorityIds =
                        transportRoute.TransitStatesData.Select(ts => ts.CompetentAuthority.Id).ToArray();
                }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Guid id, EditStateOfExportViewModel model, string submit, string country)
        {
            model.NotificationId = id;
            using (var client = apiClient())
            {
                var countries = await GetCountrySelectList(client, model.CountryId);

                // State of export is not allowed to have the same country as state of import.
                if (model.CountryId == model.StateOfImportCountryId && !string.IsNullOrWhiteSpace(country))
                {
                    RemoveModelStateErrors();
                    ModelState.AddModelError("CountryId", "Export country may not be the same as import country.");
                    EditChangeCountryPostback(model, countries);
                    return View(model);
                }

                // State of export is not allowed to have the same country as transit states.
                if (model.TransitStateCompetentAuthorityIds != null
                    && model.CountryId.HasValue
                    && model.TransitStateCompetentAuthorityIds.Contains(model.CountryId.Value))
                {
                    RemoveModelStateErrors();
                    ModelState.AddModelError("CountryId", "Export country may not be the same as a transit state country.");
                    EditChangeCountryPostback(model, countries);
                    return View(model);
                }

                // User is attempting to submit their completed edit but has errors.
                if (!ModelState.IsValid && !string.IsNullOrWhiteSpace(submit))
                {
                    await EditInvalidModelStatePostback(model, countries, client);
                    return View(model);
                }

                // User is changing country.
                if (!string.IsNullOrWhiteSpace(country))
                {
                    await EditSelectCountryPostback(model, countries, client);

                    return View(model);
                }

                // User is submitting the form, save the edit then return to summary.
                try
                {
                    var result = await client.SendAsync(User.GetAccessToken(),
                        new EditStateOfExportForNotification(id,
                        model.CountryId.Value,
                        model.ExitPointId.Value,
                        model.CompetentAuthorities.SelectedValue));
                    return RedirectToAction("Summary", "TransportRoute", new { id });
                }
                catch (ApiException)
                {
                    // Cannot run await in catch until C# 6.
                    ModelState.AddModelError("All", "Error updating the State of Export");
                }
                await EditInvalidModelStatePostback(model, countries, client);
                return View(model);
            }
        }

        private async Task EditInvalidModelStatePostback(EditStateOfExportViewModel model, SelectList countries,
            IIwsClient client)
        {
            model.Countries = countries;
            model.CompetentAuthorities =
                await GetCompetentAuthorities(client, model.CountryId, model.CompetentAuthorities.SelectedValue);
            var exitPoints = await GetExitPointsSelectList(client, model.CountryId, null);
            model.ExitPoints = exitPoints;
        }

        private void EditChangeCountryPostback(EditStateOfExportViewModel model, SelectList countries)
        {
            model.ShowNextSection = false;
            model.Countries = countries;
            model.CompetentAuthorityId = null;
            model.ExitPointId = null;
        }

        private async Task EditSelectCountryPostback(EditStateOfExportViewModel model, SelectList countries, IIwsClient client)
        {
            RemoveModelStateErrors();
            model.ShowNextSection = true;
            var competentAuthorities = await GetCompetentAuthorities(client, model.CountryId, model.CompetentAuthorityId);
            var exitPoints = await GetExitPointsSelectList(client, model.CountryId, model.ExitPointId);
            model.Countries = countries;
            model.CompetentAuthorities = competentAuthorities;
            model.ExitPoints = exitPoints;
        }

        private async Task<SelectList> GetExitPointsSelectList(IIwsClient client, Guid? countryId, Guid? selectedValue)
        {
            if (!countryId.HasValue)
            {
                return null;
            }

            var exitPoints = await client.SendAsync(new GetEntryOrExitPointsByCountry(countryId.Value));

            return (selectedValue.HasValue) ? new SelectList(exitPoints, "Id", "Name", selectedValue.Value) : new SelectList(exitPoints, "Id", "Name");
        }

        private async Task<SelectList> GetCountrySelectList(IIwsClient client, Guid? selectedValue)
        {
            var countries = await client.SendAsync(new GetCountries());
            SelectList countrySelectList = null;

            if (selectedValue.HasValue)
            {
                countrySelectList = new SelectList(countries, "Id", "Name", selectedValue.Value);
            }
            else
            {
                countrySelectList = new SelectList(countries, "Id", "Name");
            }

            return countrySelectList;
        }

        private async Task<StringGuidRadioButtons> GetCompetentAuthorities(IIwsClient client, Guid? countryId, Guid? selectedValue)
        {
            if (!countryId.HasValue)
            {
                return null;
            }

            var competentAuthorities = await client.SendAsync(new GetCompetentAuthoritiesByCountry(countryId.Value));

            var radioButtons = new StringGuidRadioButtons(competentAuthorities.Select(
                ca =>
                    new KeyValuePair<string, Guid>(string.Format("{0} - {1}", ca.Code, ca.Name), ca.Id)));

            if (selectedValue.HasValue)
            {
                radioButtons.SelectedValue = selectedValue.Value;
            }

            return radioButtons;
        }

        private void RemoveModelStateErrors()
        {
            foreach (var modelValue in ModelState.Values)
            {
                modelValue.Errors.Clear();
            }
        }

        private async Task BindExitOrEntryPointSelectList(Func<IIwsClient> apiClient, Guid countryId)
        {
            using (var client = apiClient())
            {
                await BindExitOrEntryPointSelectList(client, countryId);
            }
        }

        private async Task BindExitOrEntryPointSelectList(IIwsClient client, Guid countryId)
        {
            var entryOrExitPoints = await client.SendAsync(new GetEntryOrExitPointsByCountry(countryId));
            ViewBag.EntryOrExitPoints = new SelectList(entryOrExitPoints, "Id", "Name");
        }
    }
}