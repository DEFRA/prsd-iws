namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
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

        public TransitStateController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            using (var client = apiClient())
            {
                await this.BindCountryList(client, false);

                TransitStateViewModel model = await PrepareViewModel(id, client);

                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add(Guid id,
            TransitStateViewModel model)
        {
            using (var client = apiClient())
            {
                await this.BindCountryList(client);

                if (!ModelState.IsValid)
                {
                    if (model.IsCountrySelected)
                    {
                        await BindExitOrEntryPointSelectList(client, model.CountryId.Value);
                    }

                    return View(model);
                }

                if (model.IsCountrySelected)
                {
                    try
                    {
                        await BindExitOrEntryPointSelectList(client, model.CountryId.Value);

                        var request = new AddTransitStateToNotification(id,
                            model.CountryId.Value,
                            model.EntryPointId.Value,
                            model.ExitPointId.Value,
                            model.CompetentAuthorities.SelectedValue);

                        await client.SendAsync(User.GetAccessToken(), request);
                    }
                    catch (ApiException)
                    {
                        ModelState.AddModelError(string.Empty, "Error saving this record.");
                        return View(model);
                    }
                }
                else
                {
                    return await CountrySelectedPostback(model);
                }

                return RedirectToAction("Summary", "TransportRoute", new { id });
            }
        }

        private async Task<TransitStateViewModel> PrepareViewModel(Guid id, IIwsClient client)
        {
            TransportRouteData transportRoute = await client.SendAsync(User.GetAccessToken(),
                    new GetTransportRouteSummaryForNotification(id));

            var model = new TransitStateViewModel
            {
                IsCountrySelected = false,
                Countries = ViewBag.Countries as SelectList
            };

            // To help with validation get all the transport route data to store it in the view
            if (transportRoute.StateOfExportData != null && transportRoute.StateOfExportData.Country != null)
            {
                model.StateOfExportCountryId = transportRoute.StateOfExportData.Country.Id;
            }

            if (transportRoute.StateOfImportData != null && transportRoute.StateOfImportData.Country != null)
            {
                model.StateOfImportCountryId = transportRoute.StateOfImportData.Country.Id;
            }

            if (transportRoute.TransitStatesData != null)
            {
                model.TransitStateCountryIds =
                    transportRoute.TransitStatesData.Select(ts => ts.Country.Id).ToArray();
            }

            return model;
        }

        private async Task<ActionResult> CountrySelectedPostback(TransitStateViewModel model)
        {
            this.RemoveModelStateErrors();

            if (!model.CountryId.HasValue)
            {
                ModelState.AddModelError("CountryId", "Please select a country.");
                return View(model);
            }

            using (var client = apiClient())
            {
                var competentAuthorities = await client.SendAsync(new GetCompetentAuthoritiesByCountry(model.CountryId.Value));
                var radioButtons = competentAuthorities.Select(ca => new KeyValuePair<string, Guid>(string.Format("{0} - {1}", ca.Code, ca.Name), ca.Id));
                model.CompetentAuthorities = new StringGuidRadioButtons(radioButtons);
                model.IsCountrySelected = true;

                await BindExitOrEntryPointSelectList(client, (Guid)model.CountryId);
            }

            return View(model);
        }

        private async Task BindExitOrEntryPointSelectList(IIwsClient client, Guid countryId)
        {
            var entryOrExitPoints = await client.SendAsync(new GetEntryOrExitPointsByCountry(countryId));
            ViewBag.EntryOrExitPoints = new SelectList(entryOrExitPoints, "Id", "Name");
        }
    }
}